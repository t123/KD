using Fluxor;
using KD.Infrastructure.k8s.Fluxor;
using KD.UI.Code;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KD.UI.Components.Pages.k8s;

public class BaseView : BaseK8s
{
    protected MudMenu? _contextMenu;
    protected Action? _refreshAction;
    protected bool _isRefreshEnabled = true;

    public TimerPlus? Timer { get; set; }
    public const int TimerRefreshInterval = 60;
    public const string TableHeight = @"calc(100vh - 200px);";

    public bool EnableContextMenu { get; set; } = true;
    public string SearchText { get; set; } = string.Empty;
    public bool ShowSearch { get; set; } = true;
    public bool IsRefreshing { get; set; } = false;

    [Inject]
    public IState<NamespacesConfigState> NamespacesState { get; set; }

    [Parameter]
    public required TabModel Tab { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Timer = new TimerPlus(TimeSpan.FromSeconds(TimerRefreshInterval));
        Timer.AutoReset = true;
        Timer.Elapsed += (sender, e) =>
        {
            Fetch();
        };
    }

    protected async Task PerformSearch(string s)
    {
        SearchText = s;
    }

    protected virtual void Fetch()
    {
        if (_isRefreshEnabled)
        {
            Timer?.Stop();
        }

        _refreshAction?.Invoke();

        if (_isRefreshEnabled)
        {
            Timer?.Start();
        }
    }

    protected override ValueTask DisposeAsyncCore(bool disposing)
    {
        if (disposing)
        {
            Timer?.Stop();
            Timer?.Dispose();
            Timer = null;
        }

        return base.DisposeAsyncCore(disposing);
    }
}
