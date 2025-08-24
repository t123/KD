using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record ServiceViewState : GenericFeatureState<ServiceViewModel>;
public record FetchKubernetesServiceAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ServiceViewModel>(Tab, CancellationToken);
public record FetchKubernetesServiceActionResult(TabModel Tab, IEnumerable<ServiceViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ServiceViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ServiceViewState ReduceFetchKubernetesServiceAction(ServiceViewState state, FetchKubernetesServiceAction action)
        => FetchStateBegin(state, action) as ServiceViewState;

    [ReducerMethod]
    public static ServiceViewState ReduceFetchKubernetesServiceActionResult(ServiceViewState state, FetchKubernetesServiceActionResult action)
        => FetchStateResult(state, action) as ServiceViewState;
}

internal class ServiceViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public ServiceViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesServiceAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        var items = await _viewStateHelper.GetServices(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Service, items);
        dispatcher.Dispatch(new FetchKubernetesServiceActionResult(action.Tab, items ?? []));
    }
}
