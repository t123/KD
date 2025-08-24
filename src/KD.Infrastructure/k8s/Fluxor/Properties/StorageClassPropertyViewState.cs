using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record StorageClassPropertyViewState : GenericPropertyFeatureState<StorageClassPropertyViewModel>;
public record FetchKubernetesStorageClassPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<StorageClassPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesStorageClassPropertyActionResult(TabModel Tab, StorageClassPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<StorageClassPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static StorageClassPropertyViewState ReduceFetchKubernetesStorageClassPropertyAction(StorageClassPropertyViewState state, FetchKubernetesStorageClassPropertyAction action)
        => (FetchStateBegin(state, action) as StorageClassPropertyViewState)!;

    [ReducerMethod]
    public static StorageClassPropertyViewState ReduceFetchKubernetesStorageClassPropertyActionResult(StorageClassPropertyViewState state, FetchKubernetesStorageClassPropertyActionResult action)
        => (FetchStateResult(state, action) as StorageClassPropertyViewState)!;
}

internal class StorageClassPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public StorageClassPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesStorageClassPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new StorageClassPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesStorageClassPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}