using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record PersistentVolumeClaimViewState : GenericFeatureState<PersistentVolumeClaimViewModel>;
public record FetchKubernetesPersistentVolumeClaimAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PersistentVolumeClaimViewModel>(Tab, CancellationToken);
public record FetchKubernetesPersistentVolumeClaimActionResult(TabModel Tab, IEnumerable<PersistentVolumeClaimViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PersistentVolumeClaimViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PersistentVolumeClaimViewState ReduceFetchKubernetesPersistentVolumeClaimAction(PersistentVolumeClaimViewState state, FetchKubernetesPersistentVolumeClaimAction action)
        => FetchStateBegin(state, action) as PersistentVolumeClaimViewState;

    [ReducerMethod]
    public static PersistentVolumeClaimViewState ReduceFetchKubernetesPersistentVolumeClaimActionResult(PersistentVolumeClaimViewState state, FetchKubernetesPersistentVolumeClaimActionResult action)
        => FetchStateResult(state, action) as PersistentVolumeClaimViewState;
}

internal class PersistentVolumeClaimViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public PersistentVolumeClaimViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPersistentVolumeClaimAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PersistentVolumeClaimViewModel>? items = await _viewStateHelper.GetPersistentVolumeClaims(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.PersistentVolumeClaim, items);
        dispatcher.Dispatch(new FetchKubernetesPersistentVolumeClaimActionResult(action.Tab, items ?? []));
    }
}
