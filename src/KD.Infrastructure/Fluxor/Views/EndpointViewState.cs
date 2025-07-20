using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record EndpointViewState : GenericFeatureState<EndpointViewModel>;
public record FetchKubernetesEndpointAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<EndpointViewModel>(Tab, CancellationToken);
public record FetchKubernetesEndpointActionResult(Tab Tab, IEnumerable<EndpointViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<EndpointViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static EndpointViewState ReduceFetchKubernetesEndpointAction(EndpointViewState state, FetchKubernetesEndpointAction action)
        => FetchStateBegin(state, action) as EndpointViewState;

    [ReducerMethod]
    public static EndpointViewState ReduceFetchKubernetesEndpointActionResult(EndpointViewState state, FetchKubernetesEndpointActionResult action)
        => FetchStateResult(state, action) as EndpointViewState;
}

public class EndpointViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public EndpointViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesEndpointAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<EndpointViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListEndpointsForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x =>
                {
                    return new EndpointViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp());
                })
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Endpoint, items);
        dispatcher.Dispatch(new FetchKubernetesEndpointActionResult(action.Tab, items ?? []));
    }
}