using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record RoleBindingViewState : GenericFeatureState<RoleBindingViewModel>;
public record FetchKubernetesRoleBindingAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<RoleBindingViewModel>(Tab, CancellationToken);
public record FetchKubernetesRoleBindingActionResult(TabModel Tab, IEnumerable<RoleBindingViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<RoleBindingViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RoleBindingViewState ReduceFetchKubernetesRoleBindingAction(RoleBindingViewState state, FetchKubernetesRoleBindingAction action)
        => FetchStateBegin(state, action) as RoleBindingViewState;

    [ReducerMethod]
    public static RoleBindingViewState ReduceFetchKubernetesRoleBindingActionResult(RoleBindingViewState state, FetchKubernetesRoleBindingActionResult action)
        => FetchStateResult(state, action) as RoleBindingViewState;
}

internal class RoleBindingViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public RoleBindingViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesRoleBindingAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<RoleBindingViewModel>? items = await _viewStateHelper.GetRoleBindings(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ResourceQuota, items);
        dispatcher.Dispatch(new FetchKubernetesRoleBindingActionResult(action.Tab, items ?? []));
    }
}
