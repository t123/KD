using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record PortForwardingViewState : GenericFeatureState<PortForwardingViewModel>;
public record FetchKubernetesPortForwardingAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PortForwardingViewModel>(Tab, CancellationToken);
public record FetchKubernetesPortForwardingActionResult(Tab Tab, IEnumerable<PortForwardingViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PortForwardingViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PortForwardingViewState ReduceFetchKubernetesPortForwardingAction(PortForwardingViewState state, FetchKubernetesPortForwardingAction action)
        => FetchStateBegin(state, action) as PortForwardingViewState;

    [ReducerMethod]
    public static PortForwardingViewState ReduceFetchKubernetesPortForwardingActionResult(PortForwardingViewState state, FetchKubernetesPortForwardingActionResult action)
        => FetchStateResult(state, action) as PortForwardingViewState;
}

public class PortForwardingViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;

    public PortForwardingViewStateEffects(IKubernetesClientManager clientManager)
    {
        _clientManager = clientManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPortForwardingAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PortForwardingViewModel>? namespaces = null;

        try
        {
            //var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            //namespaces = (await client.ListPortForwardingForAllNamespacesAsync(cancellationToken: action.CancellationToken))
            //    .Items
            //    .Select(x => new PortForwardingViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
            //    .OrderBy(x => x.Name);
        }
        catch
        {
            namespaces = null;
        }

        dispatcher.Dispatch(new FetchKubernetesPortForwardingActionResult(action.Tab, namespaces ?? []));
    }
}
