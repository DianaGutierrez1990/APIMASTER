using System.Threading.RateLimiting;
using APIMASTER.Authentication;
using APIMASTER.Authorization;
using APIMASTER.Middleware;
using APIMASTER.Services;
using APIMASTER.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using Serilog;

// ──────────────────────────────────────────────
// 1. Serilog bootstrap
// ──────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Force Kestrel to listen on all interfaces, port 5050
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(5050);
    });

    // Serilog from appsettings
    builder.Host.UseSerilog((context, config) =>
        config.ReadFrom.Configuration(context.Configuration));

    // ──────────────────────────────────────────────
    // 2. Services
    // ──────────────────────────────────────────────

    // Controllers + JSON options
    builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(options =>
        {
            // Custom validation error response (RFC 7807)
            options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(e => e.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                var response = new
                {
                    type = "https://api.tuempresa.com/errors/validation",
                    title = "Validation error",
                    status = 400,
                    detail = "One or more fields failed validation.",
                    requestId = context.HttpContext.TraceIdentifier,
                    errors
                };

                return new BadRequestObjectResult(response);
            };
        });

    // FluentValidation
    builder.Services.AddFluentValidationAutoValidation();
    builder.Services.AddValidatorsFromAssemblyContaining<MilkCustomersRequestValidator>();

    // Database
    builder.Services.AddSingleton<IDatabaseResolver, DatabaseResolver>();

    // Application services - Dairy Mobile
    builder.Services.AddScoped<IMilkService, MilkService>();
    builder.Services.AddScoped<IHarvestService, HarvestService>();
    builder.Services.AddScoped<ICommoditiesService, CommoditiesService>();
    builder.Services.AddScoped<ITransferService, TransferService>();

    // Application services - Cross
    builder.Services.AddScoped<ICrossCaptureService, CrossCaptureService>();
    builder.Services.AddScoped<ICrossCatalogService, CrossCatalogService>();

    // Application services - Scale I
    builder.Services.AddScoped<IScaleiService, ScaleiService>();

    // Application services - Approvals
    builder.Services.AddScoped<IApprovalsService, ApprovalsService>();

    // Infrastructure services — Dropbox for all file storage
    builder.Services.AddHttpClient();
    builder.Services.AddScoped<IFileStorageService, DropboxFileStorageService>();

    // Authentication — Dev uses a fake handler; otherwise real JWT via Keycloak
    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddAuthentication(APIMASTER.Authentication.DevAuthHandler.SchemeName)
            .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions,
                       APIMASTER.Authentication.DevAuthHandler>(
                APIMASTER.Authentication.DevAuthHandler.SchemeName, _ => { });
    }
    else
    {
        builder.Services.AddJwtAuthentication(builder.Configuration);
    }

    // Authorization policies
    builder.Services.AddAuthorizationPolicies();

    // Rate Limiting
    var rateLimitConfig = builder.Configuration.GetSection("RateLimiting");
    builder.Services.AddRateLimiter(options =>
    {
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        options.AddPolicy("per-user", context =>
        {
            var userId = context.User.FindFirst("sub")?.Value ?? "anonymous";
            return RateLimitPartition.GetSlidingWindowLimiter(userId, _ =>
                new SlidingWindowRateLimiterOptions
                {
                    PermitLimit = rateLimitConfig.GetValue("PermitLimit", 200),
                    Window = TimeSpan.FromMinutes(rateLimitConfig.GetValue("WindowMinutes", 1)),
                    SegmentsPerWindow = rateLimitConfig.GetValue("SegmentsPerWindow", 4)
                });
        });
    });

    // CORS
    var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            if (allowedOrigins.Length == 1 && allowedOrigins[0] == "*")
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            }
            else
            {
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            }
        });
    });

    // Swagger
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "APIMASTER",
            Version = "v1",
            Description = "API de operaciones. El discriminador de empresa (ID_Business) viaja como parametro del request y se pasa directo a TVFs/SPs; el API no implementa logica de tenant."
        });

        // JWT auth in Swagger
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter your JWT token"
        });

        options.AddSecurityRequirement(doc =>
        {
            var scheme = new OpenApiSecuritySchemeReference("Bearer", doc);
            var requirement = new OpenApiSecurityRequirement();
            requirement.Add(scheme, new List<string>());
            return requirement;
        });
    });

    // ──────────────────────────────────────────────
    // 3. Middleware pipeline
    // ──────────────────────────────────────────────
    var app = builder.Build();

    // Order matters: security headers first, then error handling wraps everything
    app.UseMiddleware<SecurityHeadersMiddleware>();
    app.UseMiddleware<ErrorHandlingMiddleware>();

    // Swagger (all environments for now, restrict in production later)
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger(c => c.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "APIMASTER v1");
        });
    }

    app.UseHttpsRedirection();
    app.UseCors();
    app.UseRateLimiter();

    app.UseAuthentication();
    app.UseAuthorization();

    // Audit logging
    app.UseMiddleware<AuditMiddleware>();

    app.MapControllers()
       .RequireRateLimiting("per-user");

    app.UseSerilogRequestLogging();

    Log.Information("APIMASTER starting up...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    Console.WriteLine("========== FATAL ERROR ==========");
    Console.WriteLine(ex.ToString());
    Console.WriteLine("=================================");
}
finally
{
    Log.CloseAndFlush();
}

// Make Program accessible to integration tests (WebApplicationFactory<Program>)
public partial class Program { }
