using Microsoft.AspNetCore.Components;

namespace BlazorAuthorizationOptions.Components.Pages.Authenticated;

[Route(Route)]
public partial class RequiresFallbackPolicy : ComponentBase
{
    public const string Route = "/policy/fallback";
}
