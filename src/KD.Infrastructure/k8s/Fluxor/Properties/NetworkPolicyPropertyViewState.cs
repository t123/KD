using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record NetworkPolicyPropertyViewState : GenericPropertyFeatureState<NetworkPolicyPropertyViewModel>;
public record FetchKubernetesNetworkPolicyPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<NetworkPolicyPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesNetworkPolicyPropertyActionResult(TabModel Tab, NetworkPolicyPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<NetworkPolicyPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static NetworkPolicyPropertyViewState ReduceFetchKubernetesNetworkPolicyPropertyAction(NetworkPolicyPropertyViewState state, FetchKubernetesNetworkPolicyPropertyAction action)
        => (FetchStateBegin(state, action) as NetworkPolicyPropertyViewState)!;

    [ReducerMethod]
    public static NetworkPolicyPropertyViewState ReduceFetchKubernetesNetworkPolicyPropertyActionResult(NetworkPolicyPropertyViewState state, FetchKubernetesNetworkPolicyPropertyActionResult action)
        => (FetchStateResult(state, action) as NetworkPolicyPropertyViewState)!;
}

internal class NetworkPolicyPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public NetworkPolicyPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesNetworkPolicyPropertyAction action, IDispatcher dispatcher)
    {
        var networkPolicy = await _viewStateHelper.GetNetworkPolicy(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (networkPolicy != null)
        {
            var properties = new NetworkPolicyPropertyViewModel()
            {
                Created = networkPolicy.Metadata.CreationTimestamp,
                Name = networkPolicy.Metadata.Name,
                Tab = action.Tab,
                Uid = networkPolicy.Metadata.Uid,
                NetworkPolicy = networkPolicy
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesNetworkPolicyPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}