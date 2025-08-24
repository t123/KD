using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record NetworkPolicyPropertyViewState : GenericPropertyFeatureState<NetworkPolicyPropertyViewModel>;
public record FetchKubernetesNetworkPolicyPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<NetworkPolicyPropertyViewModel>(Tab, CancellationToken);
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
    private readonly IIndexManager _indexManager;

    public NetworkPolicyPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesNetworkPolicyPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new NetworkPolicyPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesNetworkPolicyPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}