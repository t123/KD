using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record ClusterRoleViewState : GenericFeatureState<ClusterRoleViewModel>;
public record FetchKubernetesClusterRoleAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ClusterRoleViewModel>(Tab, CancellationToken);
public record FetchKubernetesClusterRoleActionResult(Tab Tab, IEnumerable<ClusterRoleViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ClusterRoleViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ClusterRoleViewState ReduceFetchKubernetesClusterRoleAction(ClusterRoleViewState state, FetchKubernetesClusterRoleAction action)
        => FetchStateBegin(state, action) as ClusterRoleViewState;

    [ReducerMethod]
    public static ClusterRoleViewState ReduceFetchKubernetesClusterRoleActionResult(ClusterRoleViewState state, FetchKubernetesClusterRoleActionResult action)
        => FetchStateResult(state, action) as ClusterRoleViewState;
}

public class ClusterRoleViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public ClusterRoleViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesClusterRoleAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ClusterRoleViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListClusterRoleAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new ClusterRoleViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ClusterRole, items);
        dispatcher.Dispatch(new FetchKubernetesClusterRoleActionResult(action.Tab, items ?? []));
    }
}
