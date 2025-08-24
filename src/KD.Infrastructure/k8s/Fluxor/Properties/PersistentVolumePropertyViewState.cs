using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record PersistentVolumePropertyViewState : GenericPropertyFeatureState<PersistentVolumePropertyViewModel>;
public record FetchKubernetesPersistentVolumePropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<PersistentVolumePropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesPersistentVolumePropertyActionResult(TabModel Tab, PersistentVolumePropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<PersistentVolumePropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PersistentVolumePropertyViewState ReduceFetchKubernetesPersistentVolumePropertyAction(PersistentVolumePropertyViewState state, FetchKubernetesPersistentVolumePropertyAction action)
        => (FetchStateBegin(state, action) as PersistentVolumePropertyViewState)!;

    [ReducerMethod]
    public static PersistentVolumePropertyViewState ReduceFetchKubernetesPersistentVolumePropertyActionResult(PersistentVolumePropertyViewState state, FetchKubernetesPersistentVolumePropertyActionResult action)
        => (FetchStateResult(state, action) as PersistentVolumePropertyViewState)!;
}

internal class PersistentVolumePropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public PersistentVolumePropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesPersistentVolumePropertyAction action, IDispatcher dispatcher)
    {
        var properties = new PersistentVolumePropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesPersistentVolumePropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}