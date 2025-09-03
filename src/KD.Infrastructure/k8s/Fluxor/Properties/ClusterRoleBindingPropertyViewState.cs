using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ClusterRoleBindingPropertyViewState : GenericPropertyFeatureState<ClusterRoleBindingPropertyViewModel>;
public record FetchKubernetesClusterRoleBindingPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ClusterRoleBindingPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesClusterRoleBindingPropertyActionResult(TabModel Tab, ClusterRoleBindingPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ClusterRoleBindingPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ClusterRoleBindingPropertyViewState ReduceFetchKubernetesClusterRoleBindingPropertyAction(ClusterRoleBindingPropertyViewState state, FetchKubernetesClusterRoleBindingPropertyAction action)
        => (FetchStateBegin(state, action) as ClusterRoleBindingPropertyViewState)!;

    [ReducerMethod]
    public static ClusterRoleBindingPropertyViewState ReduceFetchKubernetesClusterRoleBindingPropertyActionResult(ClusterRoleBindingPropertyViewState state, FetchKubernetesClusterRoleBindingPropertyActionResult action)
        => (FetchStateResult(state, action) as ClusterRoleBindingPropertyViewState)!;
}

internal class ClusterRoleBindingPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public ClusterRoleBindingPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesClusterRoleBindingPropertyAction action, IDispatcher dispatcher)
    {
        var clusterRoleBinding = await _viewStateHelper.GetClusterRoleBinding(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (clusterRoleBinding != null)
        {
            var properties = new ClusterRoleBindingPropertyViewModel()
            {
                Created = clusterRoleBinding.Metadata.CreationTimestamp,
                Name = clusterRoleBinding.Metadata.Name,
                Tab = action.Tab,
                Uid = clusterRoleBinding.Metadata.Uid,
                ClusterRoleBinding = clusterRoleBinding
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesClusterRoleBindingPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}