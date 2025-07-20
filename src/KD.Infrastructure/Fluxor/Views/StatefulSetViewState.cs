using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record StatefulSetViewState : GenericFeatureState<StatefulSetViewModel>;
public record FetchKubernetesStatefulSetAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<StatefulSetViewModel>(Tab, CancellationToken);
public record FetchKubernetesStatefulSetActionResult(Tab Tab, IEnumerable<StatefulSetViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<StatefulSetViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static StatefulSetViewState ReduceFetchKubernetesStatefulSetAction(StatefulSetViewState state, FetchKubernetesStatefulSetAction action)
        => FetchStateBegin(state, action) as StatefulSetViewState;

    [ReducerMethod]
    public static StatefulSetViewState ReduceFetchKubernetesStatefulSetActionResult(StatefulSetViewState state, FetchKubernetesStatefulSetActionResult action)
        => FetchStateResult(state, action) as StatefulSetViewState;
}

public class StatefulSetViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public StatefulSetViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesStatefulSetAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<StatefulSetViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListStatefulSetForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new StatefulSetViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.StatefulSet, items);
        dispatcher.Dispatch(new FetchKubernetesStatefulSetActionResult(action.Tab, items ?? []));
    }
}
