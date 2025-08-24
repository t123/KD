using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record CustomResourcesDefinitionPropertyViewState : GenericPropertyFeatureState<CustomResourcesDefinitionPropertyViewModel>;
public record FetchKubernetesCustomResourcesDefinitionPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<CustomResourcesDefinitionPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesCustomResourcesDefinitionPropertyActionResult(TabModel Tab, CustomResourcesDefinitionPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<CustomResourcesDefinitionPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static CustomResourcesDefinitionPropertyViewState ReduceFetchKubernetesCustomResourcesDefinitionPropertyAction(CustomResourcesDefinitionPropertyViewState state, FetchKubernetesCustomResourcesDefinitionPropertyAction action)
        => (FetchStateBegin(state, action) as CustomResourcesDefinitionPropertyViewState)!;

    [ReducerMethod]
    public static CustomResourcesDefinitionPropertyViewState ReduceFetchKubernetesCustomResourcesDefinitionPropertyActionResult(CustomResourcesDefinitionPropertyViewState state, FetchKubernetesCustomResourcesDefinitionPropertyActionResult action)
        => (FetchStateResult(state, action) as CustomResourcesDefinitionPropertyViewState)!;
}

internal class CustomResourcesDefinitionPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public CustomResourcesDefinitionPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesCustomResourcesDefinitionPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new CustomResourcesDefinitionPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesCustomResourcesDefinitionPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}