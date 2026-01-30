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
}
