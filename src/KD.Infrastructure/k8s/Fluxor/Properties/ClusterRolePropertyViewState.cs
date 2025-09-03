using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ClusterRolePropertyViewState : GenericPropertyFeatureState<ClusterRolePropertyViewModel>;
public record FetchKubernetesClusterRolePropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ClusterRolePropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesClusterRolePropertyActionResult(TabModel Tab, ClusterRolePropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ClusterRolePropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ClusterRolePropertyViewState ReduceFetchKubernetesClusterRolePropertyAction(ClusterRolePropertyViewState state, FetchKubernetesClusterRolePropertyAction action)
        => (FetchStateBegin(state, action) as ClusterRolePropertyViewState)!;

    [ReducerMethod]
    public static ClusterRolePropertyViewState ReduceFetchKubernetesClusterRolePropertyActionResult(ClusterRolePropertyViewState state, FetchKubernetesClusterRolePropertyActionResult action)
        => (FetchStateResult(state, action) as ClusterRolePropertyViewState)!;
}

internal class ClusterRolePropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public ClusterRolePropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesClusterRolePropertyAction action, IDispatcher dispatcher)
    {
        var clusterRole = await _viewStateHelper.GetClusterRole(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (clusterRole != null)
        {
            var properties = new ClusterRolePropertyViewModel()
            {
                Created = clusterRole.Metadata.CreationTimestamp,
                Name = clusterRole.Metadata.Name,
                Tab = action.Tab,
                Uid = clusterRole.Metadata.Uid,
                ClusterRole = clusterRole
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesClusterRolePropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}