using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record NetworkPolicyViewState : GenericFeatureState<NetworkPolicyViewModel>;
public record FetchKubernetesNetworkPolicyAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<NetworkPolicyViewModel>(Tab, CancellationToken);
public record FetchKubernetesNetworkPolicyActionResult(Tab Tab, IEnumerable<NetworkPolicyViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<NetworkPolicyViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static NetworkPolicyViewState ReduceFetchKubernetesNetworkPolicyAction(NetworkPolicyViewState state, FetchKubernetesNetworkPolicyAction action)
        => FetchStateBegin(state, action) as NetworkPolicyViewState;

    [ReducerMethod]
    public static NetworkPolicyViewState ReduceFetchKubernetesNetworkPolicyActionResult(NetworkPolicyViewState state, FetchKubernetesNetworkPolicyActionResult action)
        => FetchStateResult(state, action) as NetworkPolicyViewState;
}

public class NetworkPolicyViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public NetworkPolicyViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesNetworkPolicyAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<NetworkPolicyViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListNetworkPolicyForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new NetworkPolicyViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.PolicyTypes.ToArray()
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.NetworkPolicy, items);
        dispatcher.Dispatch(new FetchKubernetesNetworkPolicyActionResult(action.Tab, items ?? []));
    }
}
