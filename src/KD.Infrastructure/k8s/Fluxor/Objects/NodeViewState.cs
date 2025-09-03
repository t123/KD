using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record NodeViewState : GenericFeatureState<NodeViewModel>;
public record FetchKubernetesNodeAction(TabModel Tab, string[] Namespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<NodeViewModel>(Tab, CancellationToken);
public record FetchKubernetesNodeActionResult(TabModel Tab, IEnumerable<NodeViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<NodeViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static NodeViewState ReduceFetchKubernetesNodeAction(NodeViewState state, FetchKubernetesNodeAction action)
        => FetchStateBegin(state, action) as NodeViewState;

    [ReducerMethod]
    public static NodeViewState ReduceFetchKubernetesNodeActionResult(NodeViewState state, FetchKubernetesNodeActionResult action)
        => FetchStateResult(state, action) as NodeViewState;
}

internal class NodeViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public NodeViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesNodeAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<NodeViewModel>? items = await _viewStateHelper.GetNodes(action.Tab.ContextState, action.Namespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Node, items);
        dispatcher.Dispatch(new FetchKubernetesNodeActionResult(action.Tab, items ?? []));
    }
}
