using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record JobViewState : GenericFeatureState<JobViewModel>;
public record FetchKubernetesJobAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<JobViewModel>(Tab, CancellationToken);
public record FetchKubernetesJobActionResult(Tab Tab, IEnumerable<JobViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<JobViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static JobViewState ReduceFetchKubernetesJobAction(JobViewState state, FetchKubernetesJobAction action)
        => FetchStateBegin(state, action) as JobViewState;

    [ReducerMethod]
    public static JobViewState ReduceFetchKubernetesJobActionResult(JobViewState state, FetchKubernetesJobActionResult action)
        => FetchStateResult(state, action) as JobViewState;
}

public class JobViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public JobViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesJobAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<JobViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListJobForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new JobViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Status.Succeeded,
                    x.Status.Conditions.OrderBy(y=>y.LastTransitionTime).Select(y=>y.Reason).FirstOrDefault() ?? "",
                    x.Status.Conditions.OrderBy(y=>y.LastTransitionTime).Select(y=>y.Type).FirstOrDefault() ?? ""
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Job, items);
        dispatcher.Dispatch(new FetchKubernetesJobActionResult(action.Tab, items ?? []));
    }
}
