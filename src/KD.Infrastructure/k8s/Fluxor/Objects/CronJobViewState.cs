using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record CronJobViewState : GenericFeatureState<CronJobViewModel>;
public record FetchKubernetesCronJobAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<CronJobViewModel>(Tab, CancellationToken);
public record FetchKubernetesCronJobActionResult(TabModel Tab, IEnumerable<CronJobViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<CronJobViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static CronJobViewState ReduceFetchKubernetesCronJobAction(CronJobViewState state, FetchKubernetesCronJobAction action)
        => FetchStateBegin(state, action) as CronJobViewState;

    [ReducerMethod]
    public static CronJobViewState ReduceFetchKubernetesCronJobActionResult(CronJobViewState state, FetchKubernetesCronJobActionResult action)
        => FetchStateResult(state, action) as CronJobViewState;
}

internal class CronJobViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public CronJobViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesCronJobAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        var items = await _viewStateHelper.GetCronJobs(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.CronJob, items);
        dispatcher.Dispatch(new FetchKubernetesCronJobActionResult(action.Tab, items ?? []));
    }
}
