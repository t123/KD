using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record JobPropertyViewState : GenericPropertyFeatureState<JobPropertyViewModel>;
public record FetchKubernetesJobPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<JobPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesJobPropertyActionResult(TabModel Tab, JobPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<JobPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static JobPropertyViewState ReduceFetchKubernetesJobPropertyAction(JobPropertyViewState state, FetchKubernetesJobPropertyAction action)
        => (FetchStateBegin(state, action) as JobPropertyViewState)!;

    [ReducerMethod]
    public static JobPropertyViewState ReduceFetchKubernetesJobPropertyActionResult(JobPropertyViewState state, FetchKubernetesJobPropertyActionResult action)
        => (FetchStateResult(state, action) as JobPropertyViewState)!;
}

internal class JobPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public JobPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesJobPropertyAction action, IDispatcher dispatcher)
    {
        var job = await _viewStateHelper.GetJob(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (job != null)
        {
            var properties = new JobPropertyViewModel()
            {
                Created = job.Metadata.CreationTimestamp,
                Name = job.Metadata.Name,
                Tab = action.Tab,
                Uid = job.Metadata.Uid,
                Job = job
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesJobPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}