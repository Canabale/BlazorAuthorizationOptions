using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorAuthorizationOptions.Components.Pages;

[AllowAnonymous]
[Route(Route)]
public partial class Authentication : ComponentBase
{
    public const string Route = "/authentication";  
}
