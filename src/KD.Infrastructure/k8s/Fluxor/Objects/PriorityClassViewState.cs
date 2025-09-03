using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record PriorityClassViewState : GenericFeatureState<PriorityClassViewModel>;
public record FetchKubernetesPriorityClassAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PriorityClassViewModel>(Tab, CancellationToken);
public record FetchKubernetesPriorityClassActionResult(TabModel Tab, IEnumerable<PriorityClassViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PriorityClassViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PriorityClassViewState ReduceFetchKubernetesPriorityClassAction(PriorityClassViewState state, FetchKubernetesPriorityClassAction action)
        => FetchStateBegin(state, action) as PriorityClassViewState;

    [ReducerMethod]
    public static PriorityClassViewState ReduceFetchKubernetesPriorityClassActionResult(PriorityClassViewState state, FetchKubernetesPriorityClassActionResult action)
        => FetchStateResult(state, action) as PriorityClassViewState;
}

internal class PriorityClassViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public PriorityClassViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPriorityClassAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PriorityClassViewModel>? items = await _viewStateHelper.GetPriorityClasses(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.PriorityClass, items);
        dispatcher.Dispatch(new FetchKubernetesPriorityClassActionResult(action.Tab, items ?? []));
    }
}
