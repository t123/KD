using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record StorageClassViewState : GenericFeatureState<StorageClassViewModel>;
public record FetchKubernetesStorageClassAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<StorageClassViewModel>(Tab, CancellationToken);
public record FetchKubernetesStorageClassActionResult(TabModel Tab, IEnumerable<StorageClassViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<StorageClassViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static StorageClassViewState ReduceFetchKubernetesStorageClassAction(StorageClassViewState state, FetchKubernetesStorageClassAction action)
        => FetchStateBegin(state, action) as StorageClassViewState;

    [ReducerMethod]
    public static StorageClassViewState ReduceFetchKubernetesStorageClassActionResult(StorageClassViewState state, FetchKubernetesStorageClassActionResult action)
        => FetchStateResult(state, action) as StorageClassViewState;
}

internal class StorageClassViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public StorageClassViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesStorageClassAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<StorageClassViewModel>? items = await _viewStateHelper.GetStorageClasses(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.StorageClass, items);
        dispatcher.Dispatch(new FetchKubernetesStorageClassActionResult(action.Tab, items ?? []));
    }
}
