using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorAuthorizationOptions.Components.Pages.Authenticated;

[Authorize(Roles = "Admin")]
[Route(Route)]
public partial class RequiresAdmin : ComponentBase
{
    public const string Route = "/requires/admin";
}
