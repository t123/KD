using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record PersistentVolumeViewState : GenericFeatureState<PersistentVolumeViewModel>;
public record FetchKubernetesPersistentVolumeAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PersistentVolumeViewModel>(Tab, CancellationToken);
public record FetchKubernetesPersistentVolumeActionResult(Tab Tab, IEnumerable<PersistentVolumeViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PersistentVolumeViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PersistentVolumeViewState ReduceFetchKubernetesPersistentVolumeAction(PersistentVolumeViewState state, FetchKubernetesPersistentVolumeAction action)
        => FetchStateBegin(state, action) as PersistentVolumeViewState;

    [ReducerMethod]
    public static PersistentVolumeViewState ReduceFetchKubernetesPersistentVolumeActionResult(PersistentVolumeViewState state, FetchKubernetesPersistentVolumeActionResult action)
        => FetchStateResult(state, action) as PersistentVolumeViewState;
}

public class PersistentVolumeViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public PersistentVolumeViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPersistentVolumeAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PersistentVolumeViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListPersistentVolumeAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new PersistentVolumeViewModel(
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

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.PersistentVolume, items);
        dispatcher.Dispatch(new FetchKubernetesPersistentVolumeActionResult(action.Tab, items ?? []));
    }
}
