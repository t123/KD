using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record ClusterRoleBindingViewState : GenericFeatureState<ClusterRoleBindingViewModel>;
public record FetchKubernetesClusterRoleBindingAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ClusterRoleBindingViewModel>(Tab, CancellationToken);
public record FetchKubernetesClusterRoleBindingActionResult(Tab Tab, IEnumerable<ClusterRoleBindingViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ClusterRoleBindingViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ClusterRoleBindingViewState ReduceFetchKubernetesClusterRoleBindingAction(ClusterRoleBindingViewState state, FetchKubernetesClusterRoleBindingAction action)
        => FetchStateBegin(state, action) as ClusterRoleBindingViewState;

    [ReducerMethod]
    public static ClusterRoleBindingViewState ReduceFetchKubernetesClusterRoleBindingActionResult(ClusterRoleBindingViewState state, FetchKubernetesClusterRoleBindingActionResult action)
        => FetchStateResult(state, action) as ClusterRoleBindingViewState;
}

public class ClusterRoleBindingViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public ClusterRoleBindingViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesClusterRoleBindingAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ClusterRoleBindingViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListClusterRoleBindingAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new ClusterRoleBindingViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ClusterRoleBinding, items);
        dispatcher.Dispatch(new FetchKubernetesClusterRoleBindingActionResult(action.Tab, items ?? []));
    }
}
