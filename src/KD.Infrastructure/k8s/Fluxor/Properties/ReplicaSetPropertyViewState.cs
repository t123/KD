using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ReplicaSetPropertyViewState : GenericPropertyFeatureState<ReplicaSetPropertyViewModel>;
public record FetchKubernetesReplicaSetPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ReplicaSetPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesReplicaSetPropertyActionResult(TabModel Tab, ReplicaSetPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ReplicaSetPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ReplicaSetPropertyViewState ReduceFetchKubernetesReplicaSetPropertyAction(ReplicaSetPropertyViewState state, FetchKubernetesReplicaSetPropertyAction action)
        => (FetchStateBegin(state, action) as ReplicaSetPropertyViewState)!;

    [ReducerMethod]
    public static ReplicaSetPropertyViewState ReduceFetchKubernetesReplicaSetPropertyActionResult(ReplicaSetPropertyViewState state, FetchKubernetesReplicaSetPropertyActionResult action)
        => (FetchStateResult(state, action) as ReplicaSetPropertyViewState)!;
}

internal class ReplicaSetPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public ReplicaSetPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesReplicaSetPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new ReplicaSetPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesReplicaSetPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}