using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record PriorityClassViewState : GenericFeatureState<PriorityClassViewModel>;
public record FetchKubernetesPriorityClassAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PriorityClassViewModel>(Tab, CancellationToken);
public record FetchKubernetesPriorityClassActionResult(Tab Tab, IEnumerable<PriorityClassViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PriorityClassViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PriorityClassViewState ReduceFetchKubernetesPriorityClassAction(PriorityClassViewState state, FetchKubernetesPriorityClassAction action)
        => FetchStateBegin(state, action) as PriorityClassViewState;

    [ReducerMethod]
    public static PriorityClassViewState ReduceFetchKubernetesPriorityClassActionResult(PriorityClassViewState state, FetchKubernetesPriorityClassActionResult action)
        => FetchStateResult(state, action) as PriorityClassViewState;
}

public class PriorityClassViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public PriorityClassViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPriorityClassAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PriorityClassViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListPriorityClassAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new PriorityClassViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.PriorityClass, items);
        dispatcher.Dispatch(new FetchKubernetesPriorityClassActionResult(action.Tab, items ?? []));
    }
}
