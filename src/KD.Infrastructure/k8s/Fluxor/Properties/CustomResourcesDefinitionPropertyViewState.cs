using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record CustomResourcesDefinitionPropertyViewState : GenericPropertyFeatureState<CustomResourcesDefinitionPropertyViewModel>;
public record FetchKubernetesCustomResourcesDefinitionPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<CustomResourcesDefinitionPropertyViewModel>(Tab, CancellationToken);
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
    private readonly IViewStateHelper _viewStateHelper;

    public CustomResourcesDefinitionPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesCustomResourcesDefinitionPropertyAction action, IDispatcher dispatcher)
    {
        var customResourceDefinition = await _viewStateHelper.GetCustomResourcesDefinition(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (customResourceDefinition != null)
        {
            var properties = new CustomResourcesDefinitionPropertyViewModel
            {
                Created = customResourceDefinition.Metadata.CreationTimestamp,
                Name = customResourceDefinition.Metadata.Name,
                Tab = action.Tab,
                Uid = customResourceDefinition.Metadata.Uid,
                CustomResourceDefinition = customResourceDefinition
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesCustomResourcesDefinitionPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}