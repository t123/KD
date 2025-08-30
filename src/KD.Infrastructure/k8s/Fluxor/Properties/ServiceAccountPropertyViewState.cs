using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ServiceAccountPropertyViewState : GenericPropertyFeatureState<ServiceAccountPropertyViewModel>;
public record FetchKubernetesServiceAccountPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ServiceAccountPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesServiceAccountPropertyActionResult(TabModel Tab, ServiceAccountPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ServiceAccountPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ServiceAccountPropertyViewState ReduceFetchKubernetesServiceAccountPropertyAction(ServiceAccountPropertyViewState state, FetchKubernetesServiceAccountPropertyAction action)
        => (FetchStateBegin(state, action) as ServiceAccountPropertyViewState)!;

    [ReducerMethod]
    public static ServiceAccountPropertyViewState ReduceFetchKubernetesServiceAccountPropertyActionResult(ServiceAccountPropertyViewState state, FetchKubernetesServiceAccountPropertyActionResult action)
        => (FetchStateResult(state, action) as ServiceAccountPropertyViewState)!;
}

internal class ServiceAccountPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public ServiceAccountPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesServiceAccountPropertyAction action, IDispatcher dispatcher)
    {
        var serviceAccount = await _viewStateHelper.GetServiceAccount(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (serviceAccount != null)
        {
            var properties = new ServiceAccountPropertyViewModel()
            {
                Created = serviceAccount.Metadata.CreationTimestamp,
                Name = serviceAccount.Metadata.Name,
                Tab = action.Tab,
                Uid = serviceAccount.Metadata.Uid,
                ServiceAccount = serviceAccount
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesServiceAccountPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}