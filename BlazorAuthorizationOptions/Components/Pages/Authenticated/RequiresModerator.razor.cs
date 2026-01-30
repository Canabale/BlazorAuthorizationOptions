using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorAuthorizationOptions.Components.Pages.Authenticated;

[Authorize(Roles = "Moderator")]
[Route(Route)]
public partial class RequiresModerator : ComponentBase
{
    public const string Route = "/requires/moderator";
}
