using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record ReplicaSetViewState : GenericFeatureState<ReplicaSetViewModel>;
public record FetchKubernetesReplicaSetAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ReplicaSetViewModel>(Tab, CancellationToken);
public record FetchKubernetesReplicaSetActionResult(TabModel Tab, IEnumerable<ReplicaSetViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ReplicaSetViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ReplicaSetViewState ReduceFetchKubernetesReplicaSetAction(ReplicaSetViewState state, FetchKubernetesReplicaSetAction action)
        => FetchStateBegin(state, action) as ReplicaSetViewState;

    [ReducerMethod]
    public static ReplicaSetViewState ReduceFetchKubernetesReplicaSetActionResult(ReplicaSetViewState state, FetchKubernetesReplicaSetActionResult action)
        => FetchStateResult(state, action) as ReplicaSetViewState;
}

internal class ReplicaSetViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public ReplicaSetViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesReplicaSetAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ReplicaSetViewModel>? items = await _viewStateHelper.GetReplicaSets(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ReplicaSet, items);
        dispatcher.Dispatch(new FetchKubernetesReplicaSetActionResult(action.Tab, items ?? []));
    }
}
