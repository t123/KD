using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record EventsPropertyViewState : GenericPropertyFeatureState<EventsPropertyViewModel>;
public record FetchKubernetesEventsPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<EventsPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesEventsPropertyActionResult(TabModel Tab, EventsPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<EventsPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static EventsPropertyViewState ReduceFetchKubernetesEventsPropertyAction(EventsPropertyViewState state, FetchKubernetesEventsPropertyAction action)
        => (FetchStateBegin(state, action) as EventsPropertyViewState)!;

    [ReducerMethod]
    public static EventsPropertyViewState ReduceFetchKubernetesEventsPropertyActionResult(EventsPropertyViewState state, FetchKubernetesEventsPropertyActionResult action)
        => (FetchStateResult(state, action) as EventsPropertyViewState)!;
}

internal class EventsPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public EventsPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesEventsPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new EventsPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesEventsPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}