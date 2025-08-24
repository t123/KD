using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ResourceQuotaPropertyViewState : GenericPropertyFeatureState<ResourceQuotaPropertyViewModel>;
public record FetchKubernetesResourceQuotaPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ResourceQuotaPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesResourceQuotaPropertyActionResult(TabModel Tab, ResourceQuotaPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ResourceQuotaPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ResourceQuotaPropertyViewState ReduceFetchKubernetesResourceQuotaPropertyAction(ResourceQuotaPropertyViewState state, FetchKubernetesResourceQuotaPropertyAction action)
        => (FetchStateBegin(state, action) as ResourceQuotaPropertyViewState)!;

    [ReducerMethod]
    public static ResourceQuotaPropertyViewState ReduceFetchKubernetesResourceQuotaPropertyActionResult(ResourceQuotaPropertyViewState state, FetchKubernetesResourceQuotaPropertyActionResult action)
        => (FetchStateResult(state, action) as ResourceQuotaPropertyViewState)!;
}

internal class ResourceQuotaPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public ResourceQuotaPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesResourceQuotaPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new ResourceQuotaPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesResourceQuotaPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}