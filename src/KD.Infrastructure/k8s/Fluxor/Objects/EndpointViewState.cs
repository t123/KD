using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record EndpointViewState : GenericFeatureState<EndpointViewModel>;
public record FetchKubernetesEndpointAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<EndpointViewModel>(Tab, CancellationToken);
public record FetchKubernetesEndpointActionResult(TabModel Tab, IEnumerable<EndpointViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<EndpointViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static EndpointViewState ReduceFetchKubernetesEndpointAction(EndpointViewState state, FetchKubernetesEndpointAction action)
        => FetchStateBegin(state, action) as EndpointViewState;

    [ReducerMethod]
    public static EndpointViewState ReduceFetchKubernetesEndpointActionResult(EndpointViewState state, FetchKubernetesEndpointActionResult action)
        => FetchStateResult(state, action) as EndpointViewState;
}

internal class EndpointViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public EndpointViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesEndpointAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<EndpointViewModel>? items = await _viewStateHelper.GetEndpoints(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Endpoint, items);
        dispatcher.Dispatch(new FetchKubernetesEndpointActionResult(action.Tab, items ?? []));
    }
}