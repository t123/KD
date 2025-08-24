using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record ServiceAccountViewState : GenericFeatureState<ServiceAccountViewModel>;
public record FetchKubernetesServiceAccountAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ServiceAccountViewModel>(Tab, CancellationToken);
public record FetchKubernetesServiceAccountActionResult(TabModel Tab, IEnumerable<ServiceAccountViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ServiceAccountViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ServiceAccountViewState ReduceFetchKubernetesServiceAccountAction(ServiceAccountViewState state, FetchKubernetesServiceAccountAction action)
        => FetchStateBegin(state, action) as ServiceAccountViewState;

    [ReducerMethod]
    public static ServiceAccountViewState ReduceFetchKubernetesServiceAccountActionResult(ServiceAccountViewState state, FetchKubernetesServiceAccountActionResult action)
        => FetchStateResult(state, action) as ServiceAccountViewState;
}

internal class ServiceAccountViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public ServiceAccountViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesServiceAccountAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ServiceAccountViewModel>? items = await _viewStateHelper.GetServiceAccounts(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ServiceAccount, items);
        dispatcher.Dispatch(new FetchKubernetesServiceAccountActionResult(action.Tab, items ?? []));
    }
}
