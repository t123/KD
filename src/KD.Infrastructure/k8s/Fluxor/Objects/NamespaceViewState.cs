using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record NamespaceViewState : GenericFeatureState<NamespaceViewModel>;
public record FetchKubernetesNamespaceAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<NamespaceViewModel>(Tab, CancellationToken);
public record FetchKubernetesNamespaceActionResult(TabModel Tab, IEnumerable<NamespaceViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<NamespaceViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static NamespaceViewState ReduceFetchKubernetesNamespaceAction(NamespaceViewState state, FetchKubernetesNamespaceAction action)
        => FetchStateBegin(state, action) as NamespaceViewState;

    [ReducerMethod]
    public static NamespaceViewState ReduceFetchKubernetesNamespaceActionResult(NamespaceViewState state, FetchKubernetesNamespaceActionResult action)
        => FetchStateResult(state, action) as NamespaceViewState;
}

internal class NamespaceViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public NamespaceViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesNamespaceAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<NamespaceViewModel>? items = await _viewStateHelper.GetNamespaces(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Namespace, items);
        dispatcher.Dispatch(new FetchKubernetesNamespaceActionResult(action.Tab, items ?? []));
    }
}
