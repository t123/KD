using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record DaemonSetViewState : GenericFeatureState<DaemonSetViewModel>;
public record FetchKubernetesDaemonSetAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<DaemonSetViewModel>(Tab, CancellationToken);
public record FetchKubernetesDaemonSetActionResult(TabModel Tab, IEnumerable<DaemonSetViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<DaemonSetViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static DaemonSetViewState ReduceFetchKubernetesDaemonSetAction(DaemonSetViewState state, FetchKubernetesDaemonSetAction action)
        => FetchStateBegin(state, action) as DaemonSetViewState;

    [ReducerMethod]
    public static DaemonSetViewState ReduceFetchKubernetesDaemonSetActionResult(DaemonSetViewState state, FetchKubernetesDaemonSetActionResult action)
        => FetchStateResult(state, action) as DaemonSetViewState;
}

internal class DaemonSetViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public DaemonSetViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesDaemonSetAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<DaemonSetViewModel>? items = await _viewStateHelper.GetDaemonSets(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.DaemonSet, items);
        dispatcher.Dispatch(new FetchKubernetesDaemonSetActionResult(action.Tab, items ?? []));
    }
}
