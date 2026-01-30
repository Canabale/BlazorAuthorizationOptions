using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System.Data;
using System.Security.Claims;

namespace BlazorAuthorizationOptions.Components.Pages;

[AllowAnonymous]
[Route(Route)]
public partial class Authentication : ComponentBase
{
    public const string Route = "/authentication";
    private const string AuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    private const string Issuer = "MockIssuer";

    private static ClaimsPrincipal Admin { get; } = CreateClaimsPrincipal("John", "Doe", nameof(Admin));

    private static ClaimsPrincipal Moderator { get; } = CreateClaimsPrincipal("Max", "Mustermann", nameof(Moderator));

    private static ClaimsPrincipal User { get; } = CreateClaimsPrincipal("Mario", "Rossi");

    /// <summary>Gets or sets the accessor for the current HTTP context.</summary>
    [Inject]
    private IHttpContextAccessor? HttpContextAccessor { get; set; }

    private async Task SignInAsync(ClaimsPrincipal user)
    {
        var httpContext = this.HttpContextAccessor?.HttpContext;
        if (httpContext is not null)
        {
            await httpContext.SignInAsync(AuthenticationScheme, user);
        }
    }

    private async Task SignOutAsync()
    {
        var httpContext = this.HttpContextAccessor?.HttpContext;
        if (httpContext is not null)
        {
            await httpContext.SignOutAsync(AuthenticationScheme);
        }
    }

    private static ClaimsPrincipal CreateClaimsPrincipal(string givenName, string surname, params IEnumerable<string> roles) => new ClaimsPrincipal(new ClaimsIdentity(
        claims:
        [
            new Claim(ClaimTypes.Authentication, AuthenticationScheme, ClaimValueTypes.String, Issuer, Issuer),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString(), ClaimValueTypes.String, Issuer),
            new Claim(ClaimTypes.Email, $"{givenName.ToLower()}.{surname.ToLower()}@test.com", ClaimValueTypes.String, Issuer),
            new Claim(ClaimTypes.Name, $"{givenName} {surname}", ClaimValueTypes.String, Issuer),
            new Claim(ClaimTypes.GivenName, givenName, ClaimValueTypes.String, Issuer),
            new Claim(ClaimTypes.Surname, surname, ClaimValueTypes.String, Issuer),
            .. roles?.Select(role => new Claim(ClaimTypes.Role, role, ClaimValueTypes.String, Issuer)) ?? Array.Empty<Claim>(),
        ],
        authenticationType: ClaimTypes.Authentication,
        nameType: ClaimTypes.Name,
        roleType: ClaimTypes.Role));
}
