using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record PodViewState : GenericFeatureState<PodViewModel>;
public record FetchKubernetesPodAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PodViewModel>(Tab, CancellationToken);
public record FetchKubernetesPodActionResult(TabModel Tab, IEnumerable<PodViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PodViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PodViewState ReduceFetchKubernetesPodAction(PodViewState state, FetchKubernetesPodAction action)
        => (FetchStateBegin(state, action) as PodViewState)!;

    [ReducerMethod]
    public static PodViewState ReduceFetchKubernetesPodActionResult(PodViewState state, FetchKubernetesPodActionResult action)
        => (FetchStateResult(state, action) as PodViewState)!;
}

internal class PodViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public PodViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPodAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();

        var items = await _viewStateHelper.GetPods(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Pod, items);
        var result = new FetchKubernetesPodActionResult(action.Tab, items ?? []);
        dispatcher.Dispatch(result);
    }
}