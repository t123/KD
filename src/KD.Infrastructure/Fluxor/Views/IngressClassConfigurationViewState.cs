using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record IngressClassViewState : GenericFeatureState<IngressClassViewModel>;
public record FetchKubernetesIngressClassAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<IngressClassViewModel>(Tab, CancellationToken);
public record FetchKubernetesIngressClassActionResult(Tab Tab, IEnumerable<IngressClassViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<IngressClassViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static IngressClassViewState ReduceFetchKubernetesIngressClassAction(IngressClassViewState state, FetchKubernetesIngressClassAction action)
        => FetchStateBegin(state, action) as IngressClassViewState;

    [ReducerMethod]
    public static IngressClassViewState ReduceFetchKubernetesIngressClassActionResult(IngressClassViewState state, FetchKubernetesIngressClassActionResult action)
        => FetchStateResult(state, action) as IngressClassViewState;
}

public class IngressClassViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public IngressClassViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesIngressClassAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<IngressClassViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListIngressClassAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new IngressClassViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.IngressClass, items);
        dispatcher.Dispatch(new FetchKubernetesIngressClassActionResult(action.Tab, items ?? []));
    }
}
