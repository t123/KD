using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record EventsViewState : GenericFeatureState<EventsViewModel>;
public record FetchKubernetesEventsAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<EventsViewModel>(Tab, CancellationToken);
public record FetchKubernetesEventsActionResult(TabModel Tab, IEnumerable<EventsViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<EventsViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static EventsViewState ReduceFetchKubernetesEventsAction(EventsViewState state, FetchKubernetesEventsAction action)
        => FetchStateBegin(state, action) as EventsViewState;

    [ReducerMethod]
    public static EventsViewState ReduceFetchKubernetesEventsActionResult(EventsViewState state, FetchKubernetesEventsActionResult action)
        => FetchStateResult(state, action) as EventsViewState;
}

internal class EventsViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public EventsViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesEventsAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<EventsViewModel>? items = await _viewStateHelper.GetEvents(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Event, items);
        dispatcher.Dispatch(new FetchKubernetesEventsActionResult(action.Tab, items ?? []));
    }
}
