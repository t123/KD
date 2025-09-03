using Fluxor;
using k8s;
using KD.Infrastructure.k8s.ViewModels;

namespace KD.Infrastructure.k8s.Fluxor.Misc;

[FeatureState]
public record LogViewerViewState
{
    public bool IsOpen { get; set; }
    public string LogBody { get; set; }
    public Context? Context { get; set; }
    public string? Name { get; set; }
    public string? Namespace { get; set; }
    public string? ContainerName { get; set; }
}

public record OpenLogViewerAction(TabModel Tab, string Name, string Namespace, string ContainerName, int? Since, CancellationToken CancellationToken = default);
public record UpdateLogViewerAction(Context Context, string Name, string Namespace, string ContainerName, int? Since, CancellationToken CancellationToken = default);
public record UpdateLogViewerActionResult(string LogBody, CancellationToken CancellationToken = default);
public record OpenLogViewerActionResult(Context Context, string Name, string Namespace, string ContainerName, string LogBody, CancellationToken CancellationToken = default);
public record CloseLogViewerAction(CancellationToken CancellationToken = default);

public static partial class Reducers
{
    [ReducerMethod]
    public static LogViewerViewState ReduceOpenLogViewerAction(LogViewerViewState state, OpenLogViewerAction action)
        => state with { IsOpen = true, LogBody = string.Empty, Context = null, Name = null, Namespace = null, ContainerName = null };

    [ReducerMethod]
    public static LogViewerViewState ReduceOpenLogViewerActionResult(LogViewerViewState state, OpenLogViewerActionResult action)
        => state with { IsOpen = true, LogBody = action.LogBody, Context = action.Context, Name = action.Name, Namespace = action.Namespace, ContainerName = action.ContainerName };

    [ReducerMethod]
    public static LogViewerViewState ReduceCloseLogViewerAction(LogViewerViewState state, CloseLogViewerAction action)
        => state with { IsOpen = false, LogBody = string.Empty, Context = null, Name = null, Namespace = null, ContainerName = null };
}

internal class LogViewerViewStateStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IKubernetesClientManager _clientManager;

    public LogViewerViewStateStateEffects(IViewStateHelper viewStateHelper, IKubernetesClientManager clientManager)
    {
        _viewStateHelper = viewStateHelper;
        _clientManager = clientManager;
    }

    [EffectMethod]
    public async Task HandleUpdateLogViewerAction(UpdateLogViewerAction action, IDispatcher dispatcher)
    {
        var pod = await _viewStateHelper.GetPod(action.Context, action.Namespace, action.Name, action.CancellationToken);
        var client = _clientManager.GetClient(action.Context.Name);

        var response = await
            client
            .CoreV1
             .ReadNamespacedPodLogAsync(
                pod.Metadata.Name,
                pod.Metadata.NamespaceProperty,
                container: action.ContainerName,
                follow: false,
                sinceSeconds: action.Since
            );

        string body = new StreamReader(response).ReadToEnd();
        dispatcher.Dispatch(new UpdateLogViewerActionResult(body));
    }

    [EffectMethod]
    public async Task HandleOpenLogViewerAction(OpenLogViewerAction action, IDispatcher dispatcher)
    {
        object a = new CloseLogViewerAction();

        var pod = await _viewStateHelper.GetPod(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);
        var client = _clientManager.GetClient(action.Tab.ContextState.Name);

        var response = await
            client
            .CoreV1
             .ReadNamespacedPodLogAsync(
                pod.Metadata.Name,
                pod.Metadata.NamespaceProperty,
                container: action.ContainerName,
                follow: false,
                sinceSeconds: action.Since == null ? 300 : action.Since
            );

        string body = new StreamReader(response).ReadToEnd();
        dispatcher.Dispatch(new OpenOverlayAction(a));
        dispatcher.Dispatch(new OpenLogViewerActionResult(action.Tab.ContextState, action.Name, action.Namespace, action.ContainerName, body, action.CancellationToken));
    }

    [EffectMethod]
    public async Task HandleCloseLogViewerActionAction(CloseLogViewerAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CloseOverlayAction(action.CancellationToken));
    }
}