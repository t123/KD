using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record StatefulSetViewState : GenericFeatureState<StatefulSetViewModel>;
public record FetchKubernetesStatefulSetAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<StatefulSetViewModel>(Tab, CancellationToken);
public record FetchKubernetesStatefulSetActionResult(TabModel Tab, IEnumerable<StatefulSetViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<StatefulSetViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static StatefulSetViewState ReduceFetchKubernetesStatefulSetAction(StatefulSetViewState state, FetchKubernetesStatefulSetAction action)
        => FetchStateBegin(state, action) as StatefulSetViewState;

    [ReducerMethod]
    public static StatefulSetViewState ReduceFetchKubernetesStatefulSetActionResult(StatefulSetViewState state, FetchKubernetesStatefulSetActionResult action)
        => FetchStateResult(state, action) as StatefulSetViewState;
}

internal class StatefulSetViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public StatefulSetViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesStatefulSetAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<StatefulSetViewModel>? items = await _viewStateHelper.GetStatefulSets(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.StatefulSet, items);
        dispatcher.Dispatch(new FetchKubernetesStatefulSetActionResult(action.Tab, items ?? []));
    }
}
