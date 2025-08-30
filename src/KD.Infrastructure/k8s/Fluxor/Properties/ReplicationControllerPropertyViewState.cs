using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ReplicationControllerPropertyViewState : GenericPropertyFeatureState<ReplicationControllerPropertyViewModel>;
public record FetchKubernetesReplicationControllerPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ReplicationControllerPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesReplicationControllerPropertyActionResult(TabModel Tab, ReplicationControllerPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ReplicationControllerPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ReplicationControllerPropertyViewState ReduceFetchKubernetesReplicationControllerPropertyAction(ReplicationControllerPropertyViewState state, FetchKubernetesReplicationControllerPropertyAction action)
        => (FetchStateBegin(state, action) as ReplicationControllerPropertyViewState)!;

    [ReducerMethod]
    public static ReplicationControllerPropertyViewState ReduceFetchKubernetesReplicationControllerPropertyActionResult(ReplicationControllerPropertyViewState state, FetchKubernetesReplicationControllerPropertyActionResult action)
        => (FetchStateResult(state, action) as ReplicationControllerPropertyViewState)!;
}

internal class ReplicationControllerPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public ReplicationControllerPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesReplicationControllerPropertyAction action, IDispatcher dispatcher)
    {
        var replicationController = await _viewStateHelper.GetReplicationController(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (replicationController != null)
        {
            var properties = new ReplicationControllerPropertyViewModel()
            {
                Created = replicationController.Metadata.CreationTimestamp,
                Name = replicationController.Metadata.Name,
                Tab = action.Tab,
                Uid = replicationController.Metadata.Uid,
                ReplicationController = replicationController
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesReplicationControllerPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}