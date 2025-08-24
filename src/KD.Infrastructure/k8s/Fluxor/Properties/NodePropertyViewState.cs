using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record NodePropertyViewState : GenericPropertyFeatureState<NodePropertyViewModel>;
public record FetchKubernetesNodePropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<NodePropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesNodePropertyActionResult(TabModel Tab, NodePropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<NodePropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static NodePropertyViewState ReduceFetchKubernetesNamespacePropertyAction(NodePropertyViewState state, FetchKubernetesNodePropertyAction action)
        => (FetchStateBegin(state, action) as NodePropertyViewState)!;

    [ReducerMethod]
    public static NodePropertyViewState ReduceFetchKubernetesNamespacePropertyActionResult(NodePropertyViewState state, FetchKubernetesNodePropertyActionResult action)
        => (FetchStateResult(state, action) as NodePropertyViewState)!;
}

internal class NodePropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public NodePropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesNodePropertyAction action, IDispatcher dispatcher)
    {
        var node = await _viewStateHelper.GetNode(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (node != null)
        {
            var properties = new NodePropertyViewModel()
            {
                Created = node.Metadata.CreationTimestamp,
                Name = node.Metadata.Name,
                Tab = action.Tab,
                Uid = node.Metadata.Uid,
                Node = node
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesNodePropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}