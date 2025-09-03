using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record ClusterRoleViewState : GenericFeatureState<ClusterRoleViewModel>;
public record FetchKubernetesClusterRoleAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ClusterRoleViewModel>(Tab, CancellationToken);
public record FetchKubernetesClusterRoleActionResult(TabModel Tab, IEnumerable<ClusterRoleViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ClusterRoleViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ClusterRoleViewState ReduceFetchKubernetesClusterRoleAction(ClusterRoleViewState state, FetchKubernetesClusterRoleAction action)
        => FetchStateBegin(state, action) as ClusterRoleViewState;

    [ReducerMethod]
    public static ClusterRoleViewState ReduceFetchKubernetesClusterRoleActionResult(ClusterRoleViewState state, FetchKubernetesClusterRoleActionResult action)
        => FetchStateResult(state, action) as ClusterRoleViewState;
}

internal class ClusterRoleViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public ClusterRoleViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesClusterRoleAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        var items = await _viewStateHelper.GetClusterRoles(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ClusterRole, items);
        dispatcher.Dispatch(new FetchKubernetesClusterRoleActionResult(action.Tab, items ?? []));
    }
}
