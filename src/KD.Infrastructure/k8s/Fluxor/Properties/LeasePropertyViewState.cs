using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record LeasePropertyViewState : GenericPropertyFeatureState<LeasePropertyViewModel>;
public record FetchKubernetesLeasePropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<LeasePropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesLeasePropertyActionResult(TabModel Tab, LeasePropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<LeasePropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static LeasePropertyViewState ReduceFetchKubernetesLeasePropertyAction(LeasePropertyViewState state, FetchKubernetesLeasePropertyAction action)
        => (FetchStateBegin(state, action) as LeasePropertyViewState)!;

    [ReducerMethod]
    public static LeasePropertyViewState ReduceFetchKubernetesLeasePropertyActionResult(LeasePropertyViewState state, FetchKubernetesLeasePropertyActionResult action)
        => (FetchStateResult(state, action) as LeasePropertyViewState)!;
}

internal class LeasePropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public LeasePropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesLeasePropertyAction action, IDispatcher dispatcher)
    {
        var lease = await _viewStateHelper.GetLease(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (lease != null)
        {
            var properties = new LeasePropertyViewModel()
            {
                Created = lease.Metadata.CreationTimestamp,
                Name = lease.Metadata.Name,
                Tab = action.Tab,
                Uid = lease.Metadata.Uid,
                Lease = lease
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesLeasePropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}