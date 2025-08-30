using Fluxor;
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
    private readonly IIndexManager _indexManager;

    public ClusterRoleBindingPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesClusterRoleBindingPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new ClusterRoleBindingPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesClusterRoleBindingPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}