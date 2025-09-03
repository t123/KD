using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record DaemonSetPropertyViewState : GenericPropertyFeatureState<DaemonSetPropertyViewModel>;
public record FetchKubernetesDaemonSetPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<DaemonSetPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesDaemonSetPropertyActionResult(TabModel Tab, DaemonSetPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<DaemonSetPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static DaemonSetPropertyViewState ReduceFetchKubernetesDaemonSetPropertyAction(DaemonSetPropertyViewState state, FetchKubernetesDaemonSetPropertyAction action)
        => (FetchStateBegin(state, action) as DaemonSetPropertyViewState)!;

    [ReducerMethod]
    public static DaemonSetPropertyViewState ReduceFetchKubernetesDaemonSetPropertyActionResult(DaemonSetPropertyViewState state, FetchKubernetesDaemonSetPropertyActionResult action)
        => (FetchStateResult(state, action) as DaemonSetPropertyViewState)!;
}

internal class DaemonSetPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public DaemonSetPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesDaemonSetPropertyAction action, IDispatcher dispatcher)
    {
        var daemonSet = await _viewStateHelper.GetDaemonSet(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (daemonSet != null)
        {
            var properties = new DaemonSetPropertyViewModel
            {
                Created = daemonSet.Metadata.CreationTimestamp,
                Name = daemonSet.Metadata.Name,
                Tab = action.Tab,
                Uid = daemonSet.Metadata.Uid,
                DaemonSet = daemonSet
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesDaemonSetPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}