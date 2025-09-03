using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ServicePropertyViewState : GenericPropertyFeatureState<ServicePropertyViewModel>;
public record FetchKubernetesServicePropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ServicePropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesServicePropertyActionResult(TabModel Tab, ServicePropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ServicePropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ServicePropertyViewState ReduceFetchKubernetesServicePropertyAction(ServicePropertyViewState state, FetchKubernetesServicePropertyAction action)
        => (FetchStateBegin(state, action) as ServicePropertyViewState)!;

    [ReducerMethod]
    public static ServicePropertyViewState ReduceFetchKubernetesServicePropertyActionResult(ServicePropertyViewState state, FetchKubernetesServicePropertyActionResult action)
        => (FetchStateResult(state, action) as ServicePropertyViewState)!;
}

internal class ServicePropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public ServicePropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesServicePropertyAction action, IDispatcher dispatcher)
    {
        var service = await _viewStateHelper.GetService(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (service != null)
        {
            var properties = new ServicePropertyViewModel()
            {
                Created = service.Metadata.CreationTimestamp,
                Name = service.Metadata.Name,
                Tab = action.Tab,
                Uid = service.Metadata.Uid,
                Service = service
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesServicePropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}