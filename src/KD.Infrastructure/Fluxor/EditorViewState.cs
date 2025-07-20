using Examine;
using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels;

namespace KD.Infrastructure.Fluxor;

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

public record OpenEditorAction(Tab Tab, string Name, string Namespace, CancellationToken CancellationToken = default);
public record OpenEditorActionResult(string Yaml, CancellationToken CancellationToken = default);
public record CloseEditorAction(CancellationToken CancellationToken = default);

public static partial class Reducers
{
    [ReducerMethod]
    public static EditorViewState ReduceOpenEditor(EditorViewState state, OpenEditorAction action)
        => state with { IsOpen = true };

    [ReducerMethod]
    public static EditorViewState ReduceOpenEditorResult(EditorViewState state, OpenEditorActionResult action)
        => state with { Yaml = action.Yaml };

    [ReducerMethod]
    public static EditorViewState ReduceCloseEditor(EditorViewState state, CloseEditorAction action)
        => state with { IsOpen = false };
}

public class EditorViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;

    public EditorViewStateEffects(IKubernetesClientManager clientManager)
    {
        _clientManager = clientManager;
    }

    [EffectMethod]
    public async Task HandleOpenEditorAction(OpenEditorAction action, IDispatcher dispatcher)
    {
        var client = _clientManager.GetClient(action.Tab.ContextState.Name);
        var pod = await client.CoreV1.ReadNamespacedPodAsync(action.Name, action.Namespace);
        dispatcher.Dispatch(new OpenEditorActionResult(KubernetesYaml.Serialize(pod)));
    }
}