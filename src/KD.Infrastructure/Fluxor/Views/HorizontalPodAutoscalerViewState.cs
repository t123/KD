using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record HorizontalPodAutoscalerViewState : GenericFeatureState<HorizontalPodAutoscalerViewModel>;
public record FetchKubernetesHorizontalPodAutoscalerAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<HorizontalPodAutoscalerViewModel>(Tab, CancellationToken);
public record FetchKubernetesHorizontalPodAutoscalerActionResult(Tab Tab, IEnumerable<HorizontalPodAutoscalerViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<HorizontalPodAutoscalerViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static HorizontalPodAutoscalerViewState ReduceFetchKubernetesHorizontalPodAutoscalerAction(HorizontalPodAutoscalerViewState state, FetchKubernetesHorizontalPodAutoscalerAction action)
        => FetchStateBegin(state, action) as HorizontalPodAutoscalerViewState;

    [ReducerMethod]
    public static HorizontalPodAutoscalerViewState ReduceFetchKubernetesHorizontalPodAutoscalerActionResult(HorizontalPodAutoscalerViewState state, FetchKubernetesHorizontalPodAutoscalerActionResult action)
        => FetchStateResult(state, action) as HorizontalPodAutoscalerViewState;
}

public class HorizontalPodAutoscalerViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public HorizontalPodAutoscalerViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesHorizontalPodAutoscalerAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<HorizontalPodAutoscalerViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.AutoscalingV1.ListHorizontalPodAutoscalerForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new HorizontalPodAutoscalerViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.MinReplicas,
                    x.Spec.MaxReplicas,
                    x.Status.CurrentReplicas,
                    x.Status.DesiredReplicas,
                    x.Status.CurrentCPUUtilizationPercentage
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.HorizontalPodAutoscaler, items);
        dispatcher.Dispatch(new FetchKubernetesHorizontalPodAutoscalerActionResult(action.Tab, items ?? []));
    }
}
