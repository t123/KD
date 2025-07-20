using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record StorageClassViewState : GenericFeatureState<StorageClassViewModel>;
public record FetchKubernetesStorageClassAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<StorageClassViewModel>(Tab, CancellationToken);
public record FetchKubernetesStorageClassActionResult(Tab Tab, IEnumerable<StorageClassViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<StorageClassViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static StorageClassViewState ReduceFetchKubernetesStorageClassAction(StorageClassViewState state, FetchKubernetesStorageClassAction action)
        => FetchStateBegin(state, action) as StorageClassViewState;

    [ReducerMethod]
    public static StorageClassViewState ReduceFetchKubernetesStorageClassActionResult(StorageClassViewState state, FetchKubernetesStorageClassActionResult action)
        => FetchStateResult(state, action) as StorageClassViewState;
}

public class StorageClassViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public StorageClassViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesStorageClassAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<StorageClassViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListStorageClassAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new StorageClassViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.StorageClass, items);
        dispatcher.Dispatch(new FetchKubernetesStorageClassActionResult(action.Tab, items ?? []));
    }
}
