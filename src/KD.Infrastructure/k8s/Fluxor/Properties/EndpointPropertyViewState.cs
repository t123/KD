using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record EndpointPropertyViewState : GenericPropertyFeatureState<EndpointPropertyViewModel>;
public record FetchKubernetesEndpointPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<EndpointPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesEndpointPropertyActionResult(TabModel Tab, EndpointPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<EndpointPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static EndpointPropertyViewState ReduceFetchKubernetesEndpointPropertyAction(EndpointPropertyViewState state, FetchKubernetesEndpointPropertyAction action)
        => (FetchStateBegin(state, action) as EndpointPropertyViewState)!;

    [ReducerMethod]
    public static EndpointPropertyViewState ReduceFetchKubernetesEndpointPropertyActionResult(EndpointPropertyViewState state, FetchKubernetesEndpointPropertyActionResult action)
        => (FetchStateResult(state, action) as EndpointPropertyViewState)!;
}

internal class EndpointPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public EndpointPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesEndpointPropertyAction action, IDispatcher dispatcher)
    {
        var endpoint = await _viewStateHelper.GetEndpoint(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (endpoint != null)
        {
            var properties = new EndpointPropertyViewModel()
            {
                Created = endpoint.Metadata.CreationTimestamp,
                Name = endpoint.Metadata.Name,
                Tab = action.Tab,
                Uid = endpoint.Metadata.Uid,
                Endpoint = endpoint
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesEndpointPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}