using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record NetworkPolicyViewState : GenericFeatureState<NetworkPolicyViewModel>;
public record FetchKubernetesNetworkPolicyAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<NetworkPolicyViewModel>(Tab, CancellationToken);
public record FetchKubernetesNetworkPolicyActionResult(TabModel Tab, IEnumerable<NetworkPolicyViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<NetworkPolicyViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static NetworkPolicyViewState ReduceFetchKubernetesNetworkPolicyAction(NetworkPolicyViewState state, FetchKubernetesNetworkPolicyAction action)
        => FetchStateBegin(state, action) as NetworkPolicyViewState;

    [ReducerMethod]
    public static NetworkPolicyViewState ReduceFetchKubernetesNetworkPolicyActionResult(NetworkPolicyViewState state, FetchKubernetesNetworkPolicyActionResult action)
        => FetchStateResult(state, action) as NetworkPolicyViewState;
}

internal class NetworkPolicyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public NetworkPolicyViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesNetworkPolicyAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<NetworkPolicyViewModel>? items = await _viewStateHelper.GetNetworkPolicies(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.NetworkPolicy, items);
        dispatcher.Dispatch(new FetchKubernetesNetworkPolicyActionResult(action.Tab, items ?? []));
    }
}
