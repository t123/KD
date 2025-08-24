using Fluxor;
using k8s;
using KD.Infrastructure.k8s.ViewModels;

namespace KD.Infrastructure.k8s.Fluxor.Misc;

[FeatureState]
public record EditorViewState
{
    public bool IsOpen { get; set; }
    public string Yaml { get; set; }

    //       @"apiVersion: apps/v1
    //kind: Deployment
    //metadata:
    //  name: nginx - deployment
    //  labels:
    //    app: nginx
    //spec:
    //  replicas: 3
    //  selector:
    //    matchLabels:
    //      app: nginx
    //  template:
    //    metadata:
    //      labels:
    //        app: nginx
    //    spec:
    //      containers:
    //      -name: nginx
    //        image: nginx:1.14.2
    //        ports:
    //        -containerPort: 80
    //";
}

public record OpenEditorAction(TabModel Tab, string Name, string Namespace, string Kind, CancellationToken CancellationToken = default);
public record OpenEditorActionResult(string Yaml, CancellationToken CancellationToken = default);
public record CloseEditorAction(CancellationToken CancellationToken = default);

public static partial class Reducers
{
    [ReducerMethod]
    public static EditorViewState ReduceOpenEditor(EditorViewState state, OpenEditorAction action)
        => state with { IsOpen = true, Yaml = string.Empty };

    [ReducerMethod]
    public static EditorViewState ReduceOpenEditorResult(EditorViewState state, OpenEditorActionResult action)
        => state with { Yaml = action.Yaml };

    [ReducerMethod]
    public static EditorViewState ReduceCloseEditor(EditorViewState state, CloseEditorAction action)
        => state with { IsOpen = false, Yaml = string.Empty };
}

internal class EditorViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public EditorViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleOpenEditorAction(OpenEditorAction action, IDispatcher dispatcher)
    {
        object? item = action.Kind switch
        {
            ObjectType.Pod => await _viewStateHelper.GetPod(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken),
            ObjectType.Deployment => await _viewStateHelper.GetDeployment(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken),
            ObjectType.Service => await _viewStateHelper.GetService(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken),
            ObjectType.Ingress => await _viewStateHelper.GetIngress(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken),
            ObjectType.Endpoint => await _viewStateHelper.GetEndpoint(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken),
            ObjectType.Namespace => await _viewStateHelper.GetNamespace(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken),
            _ => null
        };

        if (item != null)
        {
            string yaml = KubernetesYaml.Serialize(item) ?? string.Empty;

            object a = new CloseEditorAction();

            dispatcher.Dispatch(new OpenOverlayAction(a));
            dispatcher.Dispatch(new OpenEditorActionResult(yaml));
        }
    }

    [EffectMethod]
    public async Task HandleCloseEditorAction(CloseEditorAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CloseOverlayAction(action.CancellationToken));
    }
}