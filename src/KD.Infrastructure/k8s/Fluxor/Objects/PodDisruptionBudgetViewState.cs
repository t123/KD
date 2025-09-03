using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record PodDisruptionBudgetViewState : GenericFeatureState<PodDisruptionBudgetViewModel>;
public record FetchKubernetesPodDisruptionBudgetAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PodDisruptionBudgetViewModel>(Tab, CancellationToken);
public record FetchKubernetesPodDisruptionBudgetActionResult(TabModel Tab, IEnumerable<PodDisruptionBudgetViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PodDisruptionBudgetViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PodDisruptionBudgetViewState ReduceFetchKubernetesPodDisruptionBudgetAction(PodDisruptionBudgetViewState state, FetchKubernetesPodDisruptionBudgetAction action)
        => FetchStateBegin(state, action) as PodDisruptionBudgetViewState;

    [ReducerMethod]
    public static PodDisruptionBudgetViewState ReduceFetchKubernetesPodDisruptionBudgetActionResult(PodDisruptionBudgetViewState state, FetchKubernetesPodDisruptionBudgetActionResult action)
        => FetchStateResult(state, action) as PodDisruptionBudgetViewState;
}

internal class PodDisruptionBudgetViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public PodDisruptionBudgetViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPodDisruptionBudgetAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PodDisruptionBudgetViewModel>? items = await _viewStateHelper.GetPodDisruptionBudgets(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.PodDisruptionBudget, items);
        dispatcher.Dispatch(new FetchKubernetesPodDisruptionBudgetActionResult(action.Tab, items ?? []));
    }
}
