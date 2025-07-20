using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record ReplicationControllerViewState : GenericFeatureState<ReplicationControllerViewModel>;
public record FetchKubernetesReplicationControllerAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ReplicationControllerViewModel>(Tab, CancellationToken);
public record FetchKubernetesReplicationControllerActionResult(Tab Tab, IEnumerable<ReplicationControllerViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ReplicationControllerViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ReplicationControllerViewState ReduceFetchKubernetesReplicationControllerAction(ReplicationControllerViewState state, FetchKubernetesReplicationControllerAction action)
        => FetchStateBegin(state, action) as ReplicationControllerViewState;

    [ReducerMethod]
    public static ReplicationControllerViewState ReduceFetchKubernetesReplicationControllerActionResult(ReplicationControllerViewState state, FetchKubernetesReplicationControllerActionResult action)
        => FetchStateResult(state, action) as ReplicationControllerViewState;
}

public class ReplicationControllerViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public ReplicationControllerViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesReplicationControllerAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ReplicationControllerViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListReplicationControllerForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new ReplicationControllerViewModel(
                    x.Uid(), 
                    x.Name(), 
                    x.Namespace(), 
                    x.CreationTimestamp(),
                    x.Status.Replicas,
                    x.Status.AvailableReplicas,
                    x.Status.ReadyReplicas
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ReplicationController, items);
        dispatcher.Dispatch(new FetchKubernetesReplicationControllerActionResult(action.Tab, items ?? []));
    }
}
