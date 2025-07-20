using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record ReplicaSetViewState : GenericFeatureState<ReplicaSetViewModel>;
public record FetchKubernetesReplicaSetAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ReplicaSetViewModel>(Tab, CancellationToken);
public record FetchKubernetesReplicaSetActionResult(Tab Tab, IEnumerable<ReplicaSetViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ReplicaSetViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ReplicaSetViewState ReduceFetchKubernetesReplicaSetAction(ReplicaSetViewState state, FetchKubernetesReplicaSetAction action)
        => FetchStateBegin(state, action) as ReplicaSetViewState;

    [ReducerMethod]
    public static ReplicaSetViewState ReduceFetchKubernetesReplicaSetActionResult(ReplicaSetViewState state, FetchKubernetesReplicaSetActionResult action)
        => FetchStateResult(state, action) as ReplicaSetViewState;
}

public class ReplicaSetViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public ReplicaSetViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesReplicaSetAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ReplicaSetViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListReplicaSetForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new ReplicaSetViewModel(
                    x.Uid(), 
                    x.Name(), 
                    x.Namespace(), 
                    x.CreationTimestamp(),
                    x.Status.ReadyReplicas,
                    x.Status.Replicas,
                    x.Status.TerminatingReplicas,
                    x.Status.AvailableReplicas
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ReplicaSet, items);
        dispatcher.Dispatch(new FetchKubernetesReplicaSetActionResult(action.Tab, items ?? []));
    }
}
