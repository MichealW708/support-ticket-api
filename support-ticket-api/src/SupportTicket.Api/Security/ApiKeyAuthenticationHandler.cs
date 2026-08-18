using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace SupportTicket.Api.Security;

public sealed class ApiKeyAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IConfiguration configuration)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(ApiKeyAuthenticationDefaults.HeaderName, out var values))
            return Task.FromResult(AuthenticateResult.NoResult());

        var suppliedKey = values.ToString();
        var agentKey = configuration["Security:SupportAgentKey"];
        var adminKey = configuration["Security:AdminKey"];

        string? role = suppliedKey switch
        {
            _ when !string.IsNullOrEmpty(adminKey) && suppliedKey == adminKey => "Admin",
            _ when !string.IsNullOrEmpty(agentKey) && suppliedKey == agentKey => "Agent",
            _ => null
        };

        if (role is null)
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key."));

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, role == "Admin" ? "admin-client" : "support-client"),
            new Claim(ClaimTypes.Role, role)
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
