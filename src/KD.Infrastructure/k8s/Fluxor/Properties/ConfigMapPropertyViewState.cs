using Fluxor;
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
    private readonly IIndexManager _indexManager;

    public ConfigMapPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesConfigMapPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new ConfigMapPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesConfigMapPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}