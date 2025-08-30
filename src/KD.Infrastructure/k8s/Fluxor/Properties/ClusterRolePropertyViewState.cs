using Fluxor;
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
    private readonly IIndexManager _indexManager;

    public ClusterRolePropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesClusterRolePropertyAction action, IDispatcher dispatcher)
    {
        var properties = new ClusterRolePropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesClusterRolePropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}