using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record RoleViewState : GenericFeatureState<RoleViewModel>;
public record FetchKubernetesRoleAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<RoleViewModel>(Tab, CancellationToken);
public record FetchKubernetesRoleActionResult(Tab Tab, IEnumerable<RoleViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<RoleViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RoleViewState ReduceFetchKubernetesRoleAction(RoleViewState state, FetchKubernetesRoleAction action)
        => FetchStateBegin(state, action) as RoleViewState;

    [ReducerMethod]
    public static RoleViewState ReduceFetchKubernetesRoleActionResult(RoleViewState state, FetchKubernetesRoleActionResult action)
        => FetchStateResult(state, action) as RoleViewState;
}

public class RoleViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public RoleViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesRoleAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<RoleViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListRoleForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new RoleViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Role, items);
        dispatcher.Dispatch(new FetchKubernetesRoleActionResult(action.Tab, items ?? []));
    }
}
