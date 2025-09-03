using KD.Infrastructure.k8s.Fluxor.Objects;
using KD.Infrastructure.k8s.ViewModels.Objects;
using KD.UI.Code;
using Microsoft.AspNetCore.Components;
using Timer = System.Timers.Timer;

namespace KD.UI.Components.Components;

public partial class GridHeader<T> : IAsyncDisposable where T : IObjectViewModel
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

    [Parameter]
    public bool IsRefreshing { get; set; } = false;

    [Parameter]
    public TimerPlus? RefreshTimer { get; set; }

    private async Task TriggerDownload(object e)
    {
    }

    private async Task OnMouseOver()
    {
    }

    private Timer _timer = new Timer(TimeSpan.FromSeconds(1));

    protected override Task OnInitializedAsync()
    {
        _timer.AutoReset = true;
        _timer.Elapsed += (obj, e) => InvokeAsync(StateHasChanged);
        _timer.Start();

        return base.OnInitializedAsync();
    }

    public ValueTask DisposeAsync()
    {
        _timer?.Dispose();

        return ValueTask.CompletedTask;
    }
}