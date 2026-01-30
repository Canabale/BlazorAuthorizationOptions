using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorAuthorizationOptions.Components.Pages.Authenticated;

[Authorize(Roles = "Member")]
[Route(Route)]
public partial class RequiresMember : ComponentBase
{
    public const string Route = "/requires/member";
}
