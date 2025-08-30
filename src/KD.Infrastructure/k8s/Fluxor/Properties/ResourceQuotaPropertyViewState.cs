using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ResourceQuotaPropertyViewState : GenericPropertyFeatureState<ResourceQuotaPropertyViewModel>;
public record FetchKubernetesResourceQuotaPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ResourceQuotaPropertyViewModel>(Tab, CancellationToken);
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
    private readonly IViewStateHelper _viewStateHelper;

    public ResourceQuotaPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesResourceQuotaPropertyAction action, IDispatcher dispatcher)
    {
        var resourceQuota = await _viewStateHelper.GetResourceQuota(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (resourceQuota != null)
        {
            var properties = new ResourceQuotaPropertyViewModel()
            {
                Created = resourceQuota.Metadata.CreationTimestamp,
                Name = resourceQuota.Metadata.Name,
                Tab = action.Tab,
                Uid = resourceQuota.Metadata.Uid,
                ResourceQuota = resourceQuota
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesResourceQuotaPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}