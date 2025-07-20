using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record PersistentVolumeClaimViewState : GenericFeatureState<PersistentVolumeClaimViewModel>;
public record FetchKubernetesPersistentVolumeClaimAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PersistentVolumeClaimViewModel>(Tab, CancellationToken);
public record FetchKubernetesPersistentVolumeClaimActionResult(Tab Tab, IEnumerable<PersistentVolumeClaimViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PersistentVolumeClaimViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PersistentVolumeClaimViewState ReduceFetchKubernetesPersistentVolumeClaimAction(PersistentVolumeClaimViewState state, FetchKubernetesPersistentVolumeClaimAction action)
        => FetchStateBegin(state, action) as PersistentVolumeClaimViewState;

    [ReducerMethod]
    public static PersistentVolumeClaimViewState ReduceFetchKubernetesPersistentVolumeClaimActionResult(PersistentVolumeClaimViewState state, FetchKubernetesPersistentVolumeClaimActionResult action)
        => FetchStateResult(state, action) as PersistentVolumeClaimViewState;
}

public class PersistentVolumeClaimViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public PersistentVolumeClaimViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPersistentVolumeClaimAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PersistentVolumeClaimViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListPersistentVolumeClaimForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new PersistentVolumeClaimViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.StorageClassName
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.PersistentVolumeClaim, items);
        dispatcher.Dispatch(new FetchKubernetesPersistentVolumeClaimActionResult(action.Tab, items ?? []));
    }
}
