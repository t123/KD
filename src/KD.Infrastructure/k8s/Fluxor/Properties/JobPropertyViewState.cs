using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record JobPropertyViewState : GenericPropertyFeatureState<JobPropertyViewModel>;
public record FetchKubernetesJobPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<JobPropertyViewModel>(Tab, CancellationToken);
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
    private readonly IIndexManager _indexManager;

    public JobPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesJobPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new JobPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesJobPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}