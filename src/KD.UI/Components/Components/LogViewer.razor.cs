using Fluxor;
using Fluxor.Blazor.Web.Components;
using KD.Infrastructure.k8s.Fluxor.Misc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;
using IDispatcher = Fluxor.IDispatcher;
using Timer = System.Timers.Timer;

namespace KD.UI.Components.Components;

//TODO fix this
public partial class LogViewer : FluxorComponent, IDisposable
{
    [Inject]
    private IState<LogViewerViewState> LogViewerState { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    protected StringBuilder Value { get; set; } = new StringBuilder();
    private Timer _timer = new Timer(TimeSpan.FromSeconds(1));

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _timer.AutoReset = true;
        

        SubscribeToAction<OpenLogViewerAction>((action) =>
        {
            Value = new StringBuilder();
        });

        SubscribeToAction<OpenLogViewerActionResult>((action) =>
        {
            Value = new StringBuilder(action.LogBody);

            _timer.Elapsed += (obj, e) => Dispatcher.Dispatch(new UpdateLogViewerAction(
            LogViewerState.Value.Context,
            LogViewerState.Value.Name,
            LogViewerState.Value.Namespace,
            LogViewerState.Value.ContainerName,
            1
        ));

            _timer.Start();
        });

        SubscribeToAction<UpdateLogViewerActionResult>((action) =>
        {
            Value.Append(action.LogBody);
        });
    }

    private async Task CloseViewer(MouseEventArgs e)
    {
        _timer?.Stop();
        _timer?.Dispose();
        Dispatcher.Dispatch(new CloseLogViewerAction());
    }

    public void Dispose()
    {
        _timer?.Stop();
        _timer?.Dispose();
    }
}