using Fluxor;
using k8s;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record NamespaceViewState : GenericFeatureState<NamespaceViewModel>;
public record FetchKubernetesNamespaceAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<NamespaceViewModel>(Tab, CancellationToken);
public record FetchKubernetesNamespaceActionResult(Tab Tab, IEnumerable<NamespaceViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<NamespaceViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static NamespaceViewState ReduceFetchKubernetesNamespaceAction(NamespaceViewState state, FetchKubernetesNamespaceAction action)
        => FetchStateBegin(state, action) as NamespaceViewState;

    [ReducerMethod]
    public static NamespaceViewState ReduceFetchKubernetesNamespaceActionResult(NamespaceViewState state, FetchKubernetesNamespaceActionResult action)
        => FetchStateResult(state, action) as NamespaceViewState;
}

public class NamespaceViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public NamespaceViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesNamespaceAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();

        IEnumerable<NamespaceViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListNamespaceAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new NamespaceViewModel(x.Metadata.Uid, x.Metadata.Name, "", x.Metadata.CreationTimestamp))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Namespace, items);
        dispatcher.Dispatch(new FetchKubernetesNamespaceActionResult(action.Tab, items ?? []));
    }
}
