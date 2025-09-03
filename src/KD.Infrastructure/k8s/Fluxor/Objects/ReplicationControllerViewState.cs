using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record ReplicationControllerViewState : GenericFeatureState<ReplicationControllerViewModel>;
public record FetchKubernetesReplicationControllerAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ReplicationControllerViewModel>(Tab, CancellationToken);
public record FetchKubernetesReplicationControllerActionResult(TabModel Tab, IEnumerable<ReplicationControllerViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ReplicationControllerViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ReplicationControllerViewState ReduceFetchKubernetesReplicationControllerAction(ReplicationControllerViewState state, FetchKubernetesReplicationControllerAction action)
        => FetchStateBegin(state, action) as ReplicationControllerViewState;

    [ReducerMethod]
    public static ReplicationControllerViewState ReduceFetchKubernetesReplicationControllerActionResult(ReplicationControllerViewState state, FetchKubernetesReplicationControllerActionResult action)
        => FetchStateResult(state, action) as ReplicationControllerViewState;
}

internal class ReplicationControllerViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public ReplicationControllerViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesReplicationControllerAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ReplicationControllerViewModel>? items = await _viewStateHelper.GetReplicationControllers(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ReplicationController, items);
        dispatcher.Dispatch(new FetchKubernetesReplicationControllerActionResult(action.Tab, items ?? []));
    }
}
