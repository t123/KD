using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record RoleViewState : GenericFeatureState<RoleViewModel>;
public record FetchKubernetesRoleAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<RoleViewModel>(Tab, CancellationToken);
public record FetchKubernetesRoleActionResult(TabModel Tab, IEnumerable<RoleViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<RoleViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RoleViewState ReduceFetchKubernetesRoleAction(RoleViewState state, FetchKubernetesRoleAction action)
        => FetchStateBegin(state, action) as RoleViewState;

    [ReducerMethod]
    public static RoleViewState ReduceFetchKubernetesRoleActionResult(RoleViewState state, FetchKubernetesRoleActionResult action)
        => FetchStateResult(state, action) as RoleViewState;
}

internal class RoleViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public RoleViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesRoleAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<RoleViewModel>? items = await _viewStateHelper.GetRoles(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Role, items);
        dispatcher.Dispatch(new FetchKubernetesRoleActionResult(action.Tab, items ?? []));
    }
}
