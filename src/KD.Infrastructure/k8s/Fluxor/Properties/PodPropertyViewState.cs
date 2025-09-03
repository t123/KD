using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record PodPropertyViewState : GenericPropertyFeatureState<PodPropertyViewModel>;
public record FetchKubernetesPodPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<PodPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesPodPropertyActionResult(TabModel Tab, PodPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<PodPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PodPropertyViewState ReduceFetchKubernetesPodPropertyAction(PodPropertyViewState state, FetchKubernetesPodPropertyAction action)
        => (FetchStateBegin(state, action) as PodPropertyViewState)!;

    [ReducerMethod]
    public static PodPropertyViewState ReduceFetchKubernetesPodPropertyActionResult(PodPropertyViewState state, FetchKubernetesPodPropertyActionResult action)
        => (FetchStateResult(state, action) as PodPropertyViewState)!;
}

internal class PodPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public PodPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesPodPropertyAction action, IDispatcher dispatcher)
    {
        var pod = await _viewStateHelper.GetPod(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (pod != null)
        {
            var properties = new PodPropertyViewModel()
            {
                Created = pod.Metadata.CreationTimestamp,
                Name = pod.Name(),
                Tab = action.Tab,
                Uid = pod.Metadata.Uid,
                Pod = pod
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesPodPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}