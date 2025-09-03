using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record ResourceQuotaViewState : GenericFeatureState<ResourceQuotaViewModel>;
public record FetchKubernetesResourceQuotaAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ResourceQuotaViewModel>(Tab, CancellationToken);
public record FetchKubernetesResourceQuotaActionResult(TabModel Tab, IEnumerable<ResourceQuotaViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ResourceQuotaViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ResourceQuotaViewState ReduceFetchKubernetesResourceQuotaAction(ResourceQuotaViewState state, FetchKubernetesResourceQuotaAction action)
        => FetchStateBegin(state, action) as ResourceQuotaViewState;

    [ReducerMethod]
    public static ResourceQuotaViewState ReduceFetchKubernetesResourceQuotaActionResult(ResourceQuotaViewState state, FetchKubernetesResourceQuotaActionResult action)
        => FetchStateResult(state, action) as ResourceQuotaViewState;
}

internal class ResourceQuotaViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public ResourceQuotaViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesResourceQuotaAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ResourceQuotaViewModel>? items = await _viewStateHelper.GetResourceQuotas(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ResourceQuota, items);
        dispatcher.Dispatch(new FetchKubernetesResourceQuotaActionResult(action.Tab, items ?? []));
    }
}
