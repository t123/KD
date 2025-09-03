using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ReplicaSetPropertyViewState : GenericPropertyFeatureState<ReplicaSetPropertyViewModel>;
public record FetchKubernetesReplicaSetPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ReplicaSetPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesReplicaSetPropertyActionResult(TabModel Tab, ReplicaSetPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ReplicaSetPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ReplicaSetPropertyViewState ReduceFetchKubernetesReplicaSetPropertyAction(ReplicaSetPropertyViewState state, FetchKubernetesReplicaSetPropertyAction action)
        => (FetchStateBegin(state, action) as ReplicaSetPropertyViewState)!;

    [ReducerMethod]
    public static ReplicaSetPropertyViewState ReduceFetchKubernetesReplicaSetPropertyActionResult(ReplicaSetPropertyViewState state, FetchKubernetesReplicaSetPropertyActionResult action)
        => (FetchStateResult(state, action) as ReplicaSetPropertyViewState)!;
}

internal class ReplicaSetPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public ReplicaSetPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesReplicaSetPropertyAction action, IDispatcher dispatcher)
    {
        var replicaSet = await _viewStateHelper.GetReplicaSet(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (replicaSet != null)
        {
            var properties = new ReplicaSetPropertyViewModel()
            {
                Created = replicaSet.Metadata.CreationTimestamp,
                Name = replicaSet.Metadata.Name,
                Tab = action.Tab,
                Uid = replicaSet.Metadata.Uid,
                ReplicaSet = replicaSet
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesReplicaSetPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}