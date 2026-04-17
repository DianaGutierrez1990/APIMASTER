using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace APIMASTER.Authentication;

/// <summary>
/// Development-only auth handler that auto-authenticates every request
/// with a fixed set of claims so the API can be tested without Keycloak.
/// </summary>
public class DevAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "DevScheme";

    public DevAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "dev-user-001"),
            new Claim("sub", "dev-user-001"),
            new Claim("tenant_id", "00000000-0000-0000-0000-000000000001"),
            new Claim("tenant_name", "dev-tenant"),
            new Claim(ClaimTypes.Role, "admin"),
            new Claim(ClaimTypes.Role, "writer"),
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
