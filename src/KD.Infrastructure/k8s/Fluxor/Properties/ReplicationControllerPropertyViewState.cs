using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ReplicationControllerPropertyViewState : GenericPropertyFeatureState<ReplicationControllerPropertyViewModel>;
public record FetchKubernetesReplicationControllerPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ReplicationControllerPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesReplicationControllerPropertyActionResult(TabModel Tab, ReplicationControllerPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ReplicationControllerPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ReplicationControllerPropertyViewState ReduceFetchKubernetesReplicationControllerPropertyAction(ReplicationControllerPropertyViewState state, FetchKubernetesReplicationControllerPropertyAction action)
        => (FetchStateBegin(state, action) as ReplicationControllerPropertyViewState)!;

    [ReducerMethod]
    public static ReplicationControllerPropertyViewState ReduceFetchKubernetesReplicationControllerPropertyActionResult(ReplicationControllerPropertyViewState state, FetchKubernetesReplicationControllerPropertyActionResult action)
        => (FetchStateResult(state, action) as ReplicationControllerPropertyViewState)!;
}

internal class ReplicationControllerPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public ReplicationControllerPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesReplicationControllerPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new ReplicationControllerPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesReplicationControllerPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}