using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record LeasePropertyViewState : GenericPropertyFeatureState<LeasePropertyViewModel>;
public record FetchKubernetesLeasePropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<LeasePropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesLeasePropertyActionResult(TabModel Tab, LeasePropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<LeasePropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static LeasePropertyViewState ReduceFetchKubernetesLeasePropertyAction(LeasePropertyViewState state, FetchKubernetesLeasePropertyAction action)
        => (FetchStateBegin(state, action) as LeasePropertyViewState)!;

    [ReducerMethod]
    public static LeasePropertyViewState ReduceFetchKubernetesLeasePropertyActionResult(LeasePropertyViewState state, FetchKubernetesLeasePropertyActionResult action)
        => (FetchStateResult(state, action) as LeasePropertyViewState)!;
}

internal class LeasePropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public LeasePropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesLeasePropertyAction action, IDispatcher dispatcher)
    {
        var properties = new LeasePropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesLeasePropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}