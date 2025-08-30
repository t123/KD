using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record PersistentVolumeClaimPropertyViewState : GenericPropertyFeatureState<PersistentVolumeClaimPropertyViewModel>;
public record FetchKubernetesPersistentVolumeClaimPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<PersistentVolumeClaimPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesPersistentVolumeClaimPropertyActionResult(TabModel Tab, PersistentVolumeClaimPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<PersistentVolumeClaimPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PersistentVolumeClaimPropertyViewState ReduceFetchKubernetesPersistentVolumeClaimPropertyAction(PersistentVolumeClaimPropertyViewState state, FetchKubernetesPersistentVolumeClaimPropertyAction action)
        => (FetchStateBegin(state, action) as PersistentVolumeClaimPropertyViewState)!;

    [ReducerMethod]
    public static PersistentVolumeClaimPropertyViewState ReduceFetchKubernetesPersistentVolumeClaimPropertyActionResult(PersistentVolumeClaimPropertyViewState state, FetchKubernetesPersistentVolumeClaimPropertyActionResult action)
        => (FetchStateResult(state, action) as PersistentVolumeClaimPropertyViewState)!;
}

internal class PersistentVolumeClaimPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public PersistentVolumeClaimPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesPersistentVolumeClaimPropertyAction action, IDispatcher dispatcher)
    {
        var pvc = await _viewStateHelper.GetPersistentVolumeClaim(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (pvc != null)
        {
            var properties = new PersistentVolumeClaimPropertyViewModel()
            {
                Created = pvc.Metadata.CreationTimestamp,
                Name = pvc.Metadata.Name,
                Tab = action.Tab,
                Uid = pvc.Metadata.Uid,
                PersistentVolumeClaim = pvc
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesPersistentVolumeClaimPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}