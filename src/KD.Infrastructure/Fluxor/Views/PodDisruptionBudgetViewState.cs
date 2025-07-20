using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record PodDisruptionBudgetViewState : GenericFeatureState<PodDisruptionBudgetViewModel>;
public record FetchKubernetesPodDisruptionBudgetAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PodDisruptionBudgetViewModel>(Tab, CancellationToken);
public record FetchKubernetesPodDisruptionBudgetActionResult(Tab Tab, IEnumerable<PodDisruptionBudgetViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PodDisruptionBudgetViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PodDisruptionBudgetViewState ReduceFetchKubernetesPodDisruptionBudgetAction(PodDisruptionBudgetViewState state, FetchKubernetesPodDisruptionBudgetAction action)
        => FetchStateBegin(state, action) as PodDisruptionBudgetViewState;

    [ReducerMethod]
    public static PodDisruptionBudgetViewState ReduceFetchKubernetesPodDisruptionBudgetActionResult(PodDisruptionBudgetViewState state, FetchKubernetesPodDisruptionBudgetActionResult action)
        => FetchStateResult(state, action) as PodDisruptionBudgetViewState;
}

public class PodDisruptionBudgetViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public PodDisruptionBudgetViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPodDisruptionBudgetAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PodDisruptionBudgetViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListPodDisruptionBudgetForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new PodDisruptionBudgetViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.MinAvailable.Value,
                    x.Spec.MaxUnavailable.Value,
                    x.Status.CurrentHealthy,
                    x.Status.DesiredHealthy
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.PodDisruptionBudget, items);
        dispatcher.Dispatch(new FetchKubernetesPodDisruptionBudgetActionResult(action.Tab, items ?? []));
    }
}
