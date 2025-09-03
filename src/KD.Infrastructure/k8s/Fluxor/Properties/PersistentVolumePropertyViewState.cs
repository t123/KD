using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record PersistentVolumePropertyViewState : GenericPropertyFeatureState<PersistentVolumePropertyViewModel>;
public record FetchKubernetesPersistentVolumePropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<PersistentVolumePropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesPersistentVolumePropertyActionResult(TabModel Tab, PersistentVolumePropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<PersistentVolumePropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PersistentVolumePropertyViewState ReduceFetchKubernetesPersistentVolumePropertyAction(PersistentVolumePropertyViewState state, FetchKubernetesPersistentVolumePropertyAction action)
        => (FetchStateBegin(state, action) as PersistentVolumePropertyViewState)!;

    [ReducerMethod]
    public static PersistentVolumePropertyViewState ReduceFetchKubernetesPersistentVolumePropertyActionResult(PersistentVolumePropertyViewState state, FetchKubernetesPersistentVolumePropertyActionResult action)
        => (FetchStateResult(state, action) as PersistentVolumePropertyViewState)!;
}

internal class PersistentVolumePropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public PersistentVolumePropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesPersistentVolumePropertyAction action, IDispatcher dispatcher)
    {
        var pv = await _viewStateHelper.GetPersistentVolume(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (pv != null)
        {
            var properties = new PersistentVolumePropertyViewModel()
            {
                Created = pv.Metadata.CreationTimestamp,
                Name = pv.Metadata.Name,
                Tab = action.Tab,
                Uid = pv.Metadata.Uid,
                PersistentVolume = pv
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesPersistentVolumePropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}