using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record EventsViewState : GenericFeatureState<EventsViewModel>;
public record FetchKubernetesEventsAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<EventsViewModel>(Tab, CancellationToken);
public record FetchKubernetesEventsActionResult(Tab Tab, IEnumerable<EventsViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<EventsViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static EventsViewState ReduceFetchKubernetesEventsAction(EventsViewState state, FetchKubernetesEventsAction action)
        => FetchStateBegin(state, action) as EventsViewState;

    [ReducerMethod]
    public static EventsViewState ReduceFetchKubernetesEventsActionResult(EventsViewState state, FetchKubernetesEventsActionResult action)
        => FetchStateResult(state, action) as EventsViewState;
}

public class EventsViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public EventsViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesEventsAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<EventsViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.EventsV1.ListEventForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new EventsViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Type,
                    x.ReportingController,
                    new ResourceViewModel(x.Regarding.Uid, x.Regarding.Kind, x.Regarding.Name),
                    x.Reason,
                    x.DeprecatedCount,
                    x.DeprecatedLastTimestamp,
                    x.DeprecatedSource.Component
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Event, items);
        dispatcher.Dispatch(new FetchKubernetesEventsActionResult(action.Tab, items ?? []));
    }
}
