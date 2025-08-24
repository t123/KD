using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record HorizontalPodAutoscalerViewState : GenericFeatureState<HorizontalPodAutoscalerViewModel>;
public record FetchKubernetesHorizontalPodAutoscalerAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<HorizontalPodAutoscalerViewModel>(Tab, CancellationToken);
public record FetchKubernetesHorizontalPodAutoscalerActionResult(TabModel Tab, IEnumerable<HorizontalPodAutoscalerViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<HorizontalPodAutoscalerViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static HorizontalPodAutoscalerViewState ReduceFetchKubernetesHorizontalPodAutoscalerAction(HorizontalPodAutoscalerViewState state, FetchKubernetesHorizontalPodAutoscalerAction action)
        => FetchStateBegin(state, action) as HorizontalPodAutoscalerViewState;

    [ReducerMethod]
    public static HorizontalPodAutoscalerViewState ReduceFetchKubernetesHorizontalPodAutoscalerActionResult(HorizontalPodAutoscalerViewState state, FetchKubernetesHorizontalPodAutoscalerActionResult action)
        => FetchStateResult(state, action) as HorizontalPodAutoscalerViewState;
}

internal class HorizontalPodAutoscalerViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public HorizontalPodAutoscalerViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesHorizontalPodAutoscalerAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<HorizontalPodAutoscalerViewModel>? items = await _viewStateHelper.GetHorizontalPodAutoscalers(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.HorizontalPodAutoscaler, items);
        dispatcher.Dispatch(new FetchKubernetesHorizontalPodAutoscalerActionResult(action.Tab, items ?? []));
    }
}
