using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorAuthorizationOptions.Controllers;

[AllowAnonymous]
public class AuthenticationController : Controller
{
    public const string SignInRoute = "/sign-in";
    public const string SignOutRoute = "/sign-out";

    [Route(SignInRoute)]
    public async Task<IActionResult> SignInAsync(string loginHint)
    {
        var user = Users.FindByRole(loginHint);
        if (user is not null)
        {
            await this.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);
            return this.Redirect("/");
        }

        return Ok($"The user {loginHint} does not exist.");
    }

    [Route(SignOutRoute)]
    public async Task<IActionResult> SignOutAsync()
    {
        await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return this.Redirect("/");
    }
}

file static class Users
{
    private const string Issuer = "MockIssuer";
    
    public static ClaimsPrincipal Admin { get; } = CreateClaimsPrincipal("John", "Doe", nameof(Admin));

    public static ClaimsPrincipal Moderator { get; } = CreateClaimsPrincipal("Max", "Mustermann", nameof(Moderator));

    public static ClaimsPrincipal Member { get; } = CreateClaimsPrincipal("Mario", "Rossi", nameof(Member));

    public static ClaimsPrincipal Guest { get; } = CreateClaimsPrincipal("Jan", "Kowalski");

    public static ClaimsPrincipal? FindByRole(string role)
        => typeof(Users).GetProperty(role)?.GetValue(null) as ClaimsPrincipal;

    private static ClaimsPrincipal CreateClaimsPrincipal(string givenName, string surname, params IEnumerable<string> roles) => new ClaimsPrincipal(new ClaimsIdentity(
        claims:
        [
            new Claim(ClaimTypes.Authentication, CookieAuthenticationDefaults.AuthenticationScheme, ClaimValueTypes.String, Issuer, Issuer),
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