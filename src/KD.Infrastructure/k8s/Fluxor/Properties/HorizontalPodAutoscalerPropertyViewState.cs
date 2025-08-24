using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record HorizontalPodAutoscalerPropertyViewState : GenericPropertyFeatureState<HorizontalPodAutoscalerPropertyViewModel>;
public record FetchKubernetesHorizontalPodAutoscalerPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<HorizontalPodAutoscalerPropertyViewModel>(Tab, CancellationToken);
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
    private readonly IIndexManager _indexManager;

    public HorizontalPodAutoscalerPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesHorizontalPodAutoscalerPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new HorizontalPodAutoscalerPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesHorizontalPodAutoscalerPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}