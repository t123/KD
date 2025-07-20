using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record CronJobViewState : GenericFeatureState<CronJobViewModel>;
public record FetchKubernetesCronJobAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<CronJobViewModel>(Tab, CancellationToken);
public record FetchKubernetesCronJobActionResult(Tab Tab, IEnumerable<CronJobViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<CronJobViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static CronJobViewState ReduceFetchKubernetesCronJobAction(CronJobViewState state, FetchKubernetesCronJobAction action)
        => FetchStateBegin(state, action) as CronJobViewState;

    [ReducerMethod]
    public static CronJobViewState ReduceFetchKubernetesCronJobActionResult(CronJobViewState state, FetchKubernetesCronJobActionResult action)
        => FetchStateResult(state, action) as CronJobViewState;
}

public class CronJobViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public CronJobViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesCronJobAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<CronJobViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListCronJobForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new CronJobViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.TimeZone,
                    x.Spec.Schedule,
                    x.Spec.Suspend,
                    x.Status.Active.Any(),
                    x.Status.LastSuccessfulTime
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.CronJob, items);
        dispatcher.Dispatch(new FetchKubernetesCronJobActionResult(action.Tab, items ?? []));
    }
}
