using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record CronJobPropertyViewState : GenericPropertyFeatureState<CronJobPropertyViewModel>;
public record FetchKubernetesCronJobPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<CronJobPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesCronJobPropertyActionResult(TabModel Tab, CronJobPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<CronJobPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static CronJobPropertyViewState ReduceFetchKubernetesCronJobPropertyAction(CronJobPropertyViewState state, FetchKubernetesCronJobPropertyAction action)
        => (FetchStateBegin(state, action) as CronJobPropertyViewState)!;

    [ReducerMethod]
    public static CronJobPropertyViewState ReduceFetchKubernetesCronJobPropertyActionResult(CronJobPropertyViewState state, FetchKubernetesCronJobPropertyActionResult action)
        => (FetchStateResult(state, action) as CronJobPropertyViewState)!;
}

internal class CronJobPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public CronJobPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesCronJobPropertyAction action, IDispatcher dispatcher)
    {
        var cronJob = await _viewStateHelper.GetCronJob(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (cronJob != null)
        {
            var properties = new CronJobPropertyViewModel()
            {
                Created = cronJob.Metadata.CreationTimestamp,
                Name = cronJob.Metadata.Name,
                Tab = action.Tab,
                Uid = cronJob.Metadata.Uid,
                CronJob = cronJob
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesCronJobPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}