using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record LeaseViewState : GenericFeatureState<LeaseViewModel>;
public record FetchKubernetesLeaseAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<LeaseViewModel>(Tab, CancellationToken);
public record FetchKubernetesLeaseActionResult(TabModel Tab, IEnumerable<LeaseViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<LeaseViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static LeaseViewState ReduceFetchKubernetesLeaseAction(LeaseViewState state, FetchKubernetesLeaseAction action)
        => FetchStateBegin(state, action) as LeaseViewState;

    [ReducerMethod]
    public static LeaseViewState ReduceFetchKubernetesLeaseActionResult(LeaseViewState state, FetchKubernetesLeaseActionResult action)
        => FetchStateResult(state, action) as LeaseViewState;
}

internal class LeaseViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public LeaseViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesLeaseAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<LeaseViewModel>? items = await _viewStateHelper.GetLeases(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Lease, items);
        dispatcher.Dispatch(new FetchKubernetesLeaseActionResult(action.Tab, items ?? []));
    }
}
