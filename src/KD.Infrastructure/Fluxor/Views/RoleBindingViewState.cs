using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record RoleBindingViewState : GenericFeatureState<RoleBindingViewModel>;
public record FetchKubernetesRoleBindingAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<RoleBindingViewModel>(Tab, CancellationToken);
public record FetchKubernetesRoleBindingActionResult(Tab Tab, IEnumerable<RoleBindingViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<RoleBindingViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RoleBindingViewState ReduceFetchKubernetesRoleBindingAction(RoleBindingViewState state, FetchKubernetesRoleBindingAction action)
        => FetchStateBegin(state, action) as RoleBindingViewState;

    [ReducerMethod]
    public static RoleBindingViewState ReduceFetchKubernetesRoleBindingActionResult(RoleBindingViewState state, FetchKubernetesRoleBindingActionResult action)
        => FetchStateResult(state, action) as RoleBindingViewState;
}

public class RoleBindingViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public RoleBindingViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesRoleBindingAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<RoleBindingViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListRoleBindingForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new RoleBindingViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ResourceQuota, items);
        dispatcher.Dispatch(new FetchKubernetesRoleBindingActionResult(action.Tab, items ?? []));
    }
}
