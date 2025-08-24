using KD.Infrastructure.k8s.Fluxor.Objects;
using KD.Infrastructure.k8s.ViewModels.Objects;
using Microsoft.AspNetCore.Components;

namespace KD.UI.Components.Components;

public partial class GridHeader<T> where T : IObjectViewModel
{
    [Parameter]
    public GenericView<T>? View { get; set; }

    [Parameter]
    public required string Label { get; set; } = string.Empty;

    [Parameter]
    public required DateTime? LastUpdate { get; set; } = null;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public EventCallback OnRefresh { get; set; }

    [Parameter]
    public required int ItemCount { get; set; }

    [Parameter]
    public required bool ShowSearch { get; set; } = true;

    [Parameter]
    public EventCallback<string> OnSearch { get; set; }

    private async Task TriggerDownload(object e)
    {
    }

    private async Task OnMouseOver()
    {

    }
}