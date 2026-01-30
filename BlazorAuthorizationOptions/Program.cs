using BlazorAuthorizationOptions.Components;
using BlazorAuthorizationOptions.Components.Pages;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Connections;
using System.Collections.Immutable;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.LoginPath = Authentication.Route;
        options.LogoutPath = Authentication.Route;
        options.Cookie.IsEssential = true;
    });

builder.Services.AddAuthorization(options => 
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireRole("Member")
        .Build();

    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapStaticAssets().AllowAnonymous();
app.MapControllers();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().Add(endpointBuilder =>
{
    // Thanks @javiercn for these workarounds. <3
    var dispatcherOptions = endpointBuilder.Metadata.OfType<HttpConnectionDispatcherOptions>().FirstOrDefault();
    if (dispatcherOptions is not null)
    {
        // TODO: This is a part of the SignalR configuration.
        // Configure it properly once the underlying issue is resolved:
        // https://github.com/dotnet/aspnetcore/issues/63520
        dispatcherOptions.CloseOnAuthenticationExpiration = true;
    }

    if (endpointBuilder is RouteEndpointBuilder routeEndpointBuilder)
    {
        // TODO: This is a part of the Blazor configuration.
        // Configure it properly once the underlying issue is resolved:
        // https://github.com/dotnet/aspnetcore/issues/57193
        var whitelistedPrefixes = ImmutableArray.Create("/_blazor", "/_framework");
        var rawPattern = routeEndpointBuilder.RoutePattern!.RawText;
        if (!string.IsNullOrEmpty(rawPattern) && whitelistedPrefixes.Any(prefix => rawPattern.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
        {
            routeEndpointBuilder.Metadata.Add(new AllowAnonymousAttribute());
        }
    }
});

await app.RunAsync();