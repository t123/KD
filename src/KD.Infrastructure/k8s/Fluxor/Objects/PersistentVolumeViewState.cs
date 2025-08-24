using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record PersistentVolumeViewState : GenericFeatureState<PersistentVolumeViewModel>;
public record FetchKubernetesPersistentVolumeAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PersistentVolumeViewModel>(Tab, CancellationToken);
public record FetchKubernetesPersistentVolumeActionResult(TabModel Tab, IEnumerable<PersistentVolumeViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PersistentVolumeViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PersistentVolumeViewState ReduceFetchKubernetesPersistentVolumeAction(PersistentVolumeViewState state, FetchKubernetesPersistentVolumeAction action)
        => FetchStateBegin(state, action) as PersistentVolumeViewState;

    [ReducerMethod]
    public static PersistentVolumeViewState ReduceFetchKubernetesPersistentVolumeActionResult(PersistentVolumeViewState state, FetchKubernetesPersistentVolumeActionResult action)
        => FetchStateResult(state, action) as PersistentVolumeViewState;
}

internal class PersistentVolumeViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public PersistentVolumeViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPersistentVolumeAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PersistentVolumeViewModel>? items = await _viewStateHelper.GetPersistentVolumes(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.PersistentVolume, items);
        dispatcher.Dispatch(new FetchKubernetesPersistentVolumeActionResult(action.Tab, items ?? []));
    }
}
