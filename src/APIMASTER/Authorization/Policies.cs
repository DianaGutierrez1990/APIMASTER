namespace APIMASTER.Authorization;

public static class Policies
{
    public const string TenantReader = nameof(TenantReader);
    public const string TenantWriter = nameof(TenantWriter);
    public const string TenantAdmin = nameof(TenantAdmin);

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy(TenantReader, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("tenant_id");
            })
            .AddPolicy(TenantWriter, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("tenant_id");
                policy.RequireRole("writer", "admin");
            })
            .AddPolicy(TenantAdmin, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim("tenant_id");
                policy.RequireRole("admin");
            });

        return services;
    }
}
