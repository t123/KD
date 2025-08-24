using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record ClusterRoleBindingViewState : GenericFeatureState<ClusterRoleBindingViewModel>;
public record FetchKubernetesClusterRoleBindingAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ClusterRoleBindingViewModel>(Tab, CancellationToken);
public record FetchKubernetesClusterRoleBindingActionResult(TabModel Tab, IEnumerable<ClusterRoleBindingViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ClusterRoleBindingViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ClusterRoleBindingViewState ReduceFetchKubernetesClusterRoleBindingAction(ClusterRoleBindingViewState state, FetchKubernetesClusterRoleBindingAction action)
        => FetchStateBegin(state, action) as ClusterRoleBindingViewState;

    [ReducerMethod]
    public static ClusterRoleBindingViewState ReduceFetchKubernetesClusterRoleBindingActionResult(ClusterRoleBindingViewState state, FetchKubernetesClusterRoleBindingActionResult action)
        => FetchStateResult(state, action) as ClusterRoleBindingViewState;
}

internal class ClusterRoleBindingViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public ClusterRoleBindingViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesClusterRoleBindingAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        var items = await _viewStateHelper.GetClusterRoleBindings(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ClusterRoleBinding, items);
        dispatcher.Dispatch(new FetchKubernetesClusterRoleBindingActionResult(action.Tab, items ?? []));
    }
}
