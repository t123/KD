using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record IngressViewState : GenericFeatureState<IngressViewModel>;
public record FetchKubernetesIngressAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<IngressViewModel>(Tab, CancellationToken);
public record FetchKubernetesIngressActionResult(TabModel Tab, IEnumerable<IngressViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<IngressViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static IngressViewState ReduceFetchKubernetesIngressAction(IngressViewState state, FetchKubernetesIngressAction action)
        => FetchStateBegin(state, action) as IngressViewState;

    [ReducerMethod]
    public static IngressViewState ReduceFetchKubernetesIngressActionResult(IngressViewState state, FetchKubernetesIngressActionResult action)
        => FetchStateResult(state, action) as IngressViewState;
}

internal class IngressViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public IngressViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesIngressAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<IngressViewModel>? items = await _viewStateHelper.GetIngresses(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Ingress, items);
        dispatcher.Dispatch(new FetchKubernetesIngressActionResult(action.Tab, items ?? []));
    }
}