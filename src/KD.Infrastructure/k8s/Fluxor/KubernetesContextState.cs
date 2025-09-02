using Fluxor;
using k8s;
using KD.Infrastructure.k8s.ViewModels;
using Microsoft.Extensions.Logging;

namespace KD.Infrastructure.k8s.Fluxor;

[FeatureState]
public record KubernetesContextState
{
    public bool IsLoading { get; set; }
    public Context? CurrentContext { get; set; }
    public Context[] Contexts { get; set; }
    public Context[] UserOrderedContexts { get; set; }

    public KubernetesContextState()
    {
        IsLoading = true;
        CurrentContext = null;
        Contexts = [];
        UserOrderedContexts = [];
    }
}

public record FetchKubernetesContextAction();
public record FetchKubernetesContextActionResult(Context? CurrentContext, Context[]? Contexts, Context[]? UserOrderedContexts);
public record ChangeKubernetesContextAction(Context Context);
public record ChangeKubernetesContextActionResult(Context? Context);

public static partial class Reducers
{
    [ReducerMethod]
    public static KubernetesContextState ReduceFetchKubernetesContexts(KubernetesContextState state, FetchKubernetesContextAction action)
        => state with { IsLoading = true };

    [ReducerMethod]
    public static KubernetesContextState ReduceFetchKubernetesContextsResult(KubernetesContextState state, FetchKubernetesContextActionResult action)
    {
        var current = state.CurrentContext;
        if (current == null && action.CurrentContext == null)
        {
            current = action?.Contexts?.FirstOrDefault();
        }

        return state with
        {
            IsLoading = false,
            CurrentContext = current,
            Contexts = action?.Contexts ?? [],
            UserOrderedContexts = action?.UserOrderedContexts ?? [],
        };
    }

    [ReducerMethod]
    public static KubernetesContextState ReduceChangeKubernetesContextActionResult(KubernetesContextState state, ChangeKubernetesContextActionResult action)
        => state with { IsLoading = false, CurrentContext = action.Context, Contexts = state.Contexts };
}

internal class KubernetesContextStateEffects
{
    private readonly ContextsManager _contextsManager;
    private readonly IBackgroundTaskQueue _backgroundTaskQueue;
    private readonly IKubernetesDataLoader _dataLoader;
    private readonly IIndexManager _indexManager;
    private readonly ILogger<KubernetesContextStateEffects> _logger;

    public KubernetesContextStateEffects(
        ContextsManager contextsManager,
        IBackgroundTaskQueue backgroundTaskQueue,
        IKubernetesDataLoader dataLoader,
        IIndexManager indexManager,
        ILogger<KubernetesContextStateEffects> logger
    )
    {
        _contextsManager = contextsManager;
        _backgroundTaskQueue = backgroundTaskQueue;
        _dataLoader = dataLoader;
        _indexManager = indexManager;
        _logger = logger;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesConfigAction(FetchKubernetesContextAction action, IDispatcher dispatcher)
    {
        var contexts = _contextsManager.GetContexts();
        var context = _contextsManager.CurrentContext;

        string[] userOrder = ["minikube", "kind-kind"];
        var newContexts = new List<Context>();

        foreach (var o in userOrder)
        {
            var c = contexts.SingleOrDefault(x => x.Name == o);

            if (c != null)
            {
                newContexts.Add(c);
            }
        }

        newContexts.AddRange(contexts.Where(x => !userOrder.Contains(x.Name)));

        dispatcher.Dispatch(new FetchKubernetesContextActionResult(context, contexts, newContexts.ToArray()));
    }

    [EffectMethod]
    public async Task HandleChangeKubernetesConfigAction(ChangeKubernetesContextAction action, IDispatcher dispatcher)
    {
        var context = _contextsManager.ChangeContext(action.Context);
        dispatcher.Dispatch(new ChangeKubernetesContextActionResult(context));

        if (context != null)
        {
            await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async (ct) =>
            {
                var pods = await _dataLoader.GetPods(context, [], ct);
                _logger.LogInformation("Got pods");
                var ingresses = await _dataLoader.GetIngresses(context, [], ct);
                _logger.LogInformation("Got ingresses");

                //await _indexManager.IndexItems(context.Name, ObjectType.Pod, pods.Items);
                //await _indexManager.IndexItems(context.Name, ObjectType.Ingress, ingresses.Items);
            });
        }
    }
}