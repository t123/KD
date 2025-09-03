using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record JobViewState : GenericFeatureState<JobViewModel>;
public record FetchKubernetesJobAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<JobViewModel>(Tab, CancellationToken);
public record FetchKubernetesJobActionResult(TabModel Tab, IEnumerable<JobViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<JobViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static JobViewState ReduceFetchKubernetesJobAction(JobViewState state, FetchKubernetesJobAction action)
        => FetchStateBegin(state, action) as JobViewState;

    [ReducerMethod]
    public static JobViewState ReduceFetchKubernetesJobActionResult(JobViewState state, FetchKubernetesJobActionResult action)
        => FetchStateResult(state, action) as JobViewState;
}

internal class JobViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public JobViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesJobAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<JobViewModel>? items = await _viewStateHelper.GetJobs(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Job, items);
        dispatcher.Dispatch(new FetchKubernetesJobActionResult(action.Tab, items ?? []));
    }
}
