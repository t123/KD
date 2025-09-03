using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record HorizontalPodAutoscalerPropertyViewState : GenericPropertyFeatureState<HorizontalPodAutoscalerPropertyViewModel>;
public record FetchKubernetesHorizontalPodAutoscalerPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<HorizontalPodAutoscalerPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesHorizontalPodAutoscalerPropertyActionResult(TabModel Tab, HorizontalPodAutoscalerPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<HorizontalPodAutoscalerPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static HorizontalPodAutoscalerPropertyViewState ReduceFetchKubernetesHorizontalPodAutoscalerPropertyAction(HorizontalPodAutoscalerPropertyViewState state, FetchKubernetesHorizontalPodAutoscalerPropertyAction action)
        => (FetchStateBegin(state, action) as HorizontalPodAutoscalerPropertyViewState)!;

    [ReducerMethod]
    public static HorizontalPodAutoscalerPropertyViewState ReduceFetchKubernetesHorizontalPodAutoscalerPropertyActionResult(HorizontalPodAutoscalerPropertyViewState state, FetchKubernetesHorizontalPodAutoscalerPropertyActionResult action)
        => (FetchStateResult(state, action) as HorizontalPodAutoscalerPropertyViewState)!;
}

internal class HorizontalPodAutoscalerPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public HorizontalPodAutoscalerPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesHorizontalPodAutoscalerPropertyAction action, IDispatcher dispatcher)
    {
        var horizontalPodAutoscaler = await _viewStateHelper.GetHorizontalPodAutoscaler(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (horizontalPodAutoscaler != null)
        {
            var properties = new HorizontalPodAutoscalerPropertyViewModel
            {
                Created = horizontalPodAutoscaler.Metadata.CreationTimestamp,
                Name = horizontalPodAutoscaler.Metadata.Name,
                Tab = action.Tab,
                Uid = horizontalPodAutoscaler.Metadata.Uid,
                HorizontalPodAutoscaler = horizontalPodAutoscaler
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesHorizontalPodAutoscalerPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}