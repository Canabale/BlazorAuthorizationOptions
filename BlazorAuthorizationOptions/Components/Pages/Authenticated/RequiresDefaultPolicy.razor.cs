using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BlazorAuthorizationOptions.Components.Pages.Authenticated;

[Authorize]
[Route(Route)]
public partial class RequiresDefaultPolicy : ComponentBase
{
    public const string Route = "/policy/default";
}
