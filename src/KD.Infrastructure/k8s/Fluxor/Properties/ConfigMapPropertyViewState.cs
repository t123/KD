using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ConfigMapPropertyViewState : GenericPropertyFeatureState<ConfigMapPropertyViewModel>;
public record FetchKubernetesConfigMapPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ConfigMapPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesConfigMapPropertyActionResult(TabModel Tab, ConfigMapPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ConfigMapPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ConfigMapPropertyViewState ReduceFetchKubernetesConfigMapPropertyAction(ConfigMapPropertyViewState state, FetchKubernetesConfigMapPropertyAction action)
        => (FetchStateBegin(state, action) as ConfigMapPropertyViewState)!;

    [ReducerMethod]
    public static ConfigMapPropertyViewState ReduceFetchKubernetesConfigMapPropertyActionResult(ConfigMapPropertyViewState state, FetchKubernetesConfigMapPropertyActionResult action)
        => (FetchStateResult(state, action) as ConfigMapPropertyViewState)!;
}

internal class ConfigMapPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public ConfigMapPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesConfigMapPropertyAction action, IDispatcher dispatcher)
    {
        var configMap = await _viewStateHelper.GetConfigMap(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (configMap != null)
        {
            var properties = new ConfigMapPropertyViewModel()
            {
                Created = configMap.Metadata.CreationTimestamp,
                Name = configMap.Metadata.Name,
                Tab = action.Tab,
                Uid = configMap.Metadata.Uid,
                ConfigMap = configMap
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesConfigMapPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}