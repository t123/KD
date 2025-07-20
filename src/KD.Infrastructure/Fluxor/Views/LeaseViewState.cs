using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record LeaseViewState : GenericFeatureState<LeaseViewModel>;
public record FetchKubernetesLeaseAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<LeaseViewModel>(Tab, CancellationToken);
public record FetchKubernetesLeaseActionResult(Tab Tab, IEnumerable<LeaseViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<LeaseViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static LeaseViewState ReduceFetchKubernetesLeaseAction(LeaseViewState state, FetchKubernetesLeaseAction action)
        => FetchStateBegin(state, action) as LeaseViewState;

    [ReducerMethod]
    public static LeaseViewState ReduceFetchKubernetesLeaseActionResult(LeaseViewState state, FetchKubernetesLeaseActionResult action)
        => FetchStateResult(state, action) as LeaseViewState;
}

public class LeaseViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public LeaseViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesLeaseAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<LeaseViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListLeaseForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new LeaseViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Lease, items);
        dispatcher.Dispatch(new FetchKubernetesLeaseActionResult(action.Tab, items ?? []));
    }
}
