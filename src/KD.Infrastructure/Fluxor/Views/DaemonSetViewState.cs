using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record DaemonSetViewState : GenericFeatureState<DaemonSetViewModel>;
public record FetchKubernetesDaemonSetAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<DaemonSetViewModel>(Tab, CancellationToken);
public record FetchKubernetesDaemonSetActionResult(Tab Tab, IEnumerable<DaemonSetViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<DaemonSetViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static DaemonSetViewState ReduceFetchKubernetesDaemonSetAction(DaemonSetViewState state, FetchKubernetesDaemonSetAction action)
        => FetchStateBegin(state, action) as DaemonSetViewState;

    [ReducerMethod]
    public static DaemonSetViewState ReduceFetchKubernetesDaemonSetActionResult(DaemonSetViewState state, FetchKubernetesDaemonSetActionResult action)
        => FetchStateResult(state, action) as DaemonSetViewState;
}

public class DaemonSetViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public DaemonSetViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesDaemonSetAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<DaemonSetViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListDaemonSetForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new DaemonSetViewModel(
                    x.Uid(), 
                    x.Name(), 
                    x.Namespace(), 
                    x.CreationTimestamp(),
                    x.Status.DesiredNumberScheduled,
                    x.Status.CurrentNumberScheduled,
                    x.Status.NumberReady,
                    x.Status.UpdatedNumberScheduled,
                    x.Status.NumberAvailable
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.DaemonSet, items);
        dispatcher.Dispatch(new FetchKubernetesDaemonSetActionResult(action.Tab, items ?? []));
    }
}
