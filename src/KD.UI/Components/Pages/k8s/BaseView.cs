using Fluxor;
using KD.Infrastructure.k8s.Fluxor;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KD.UI.Components.Pages.k8s;

public class BaseView : BaseK8s
{
    protected MudMenu? _contextMenu;

    public bool EnableContextMenu { get; set; } = true;
    public string SearchText { get; set; } = string.Empty;
    public bool ShowSearch { get; set; } = true;

    [Inject]
    public IState<NamespacesConfigState> NamespacesState { get; set; }

    [Parameter]
    public required TabModel Tab { get; set; }

    protected async Task PerformSearch(string s)
    {
        SearchText = s;
    }
}
