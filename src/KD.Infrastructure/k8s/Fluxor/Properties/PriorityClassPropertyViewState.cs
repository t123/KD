using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record PriorityClassPropertyViewState : GenericPropertyFeatureState<PriorityClassPropertyViewModel>;
public record FetchKubernetesPriorityClassPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<PriorityClassPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesPriorityClassPropertyActionResult(TabModel Tab, PriorityClassPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<PriorityClassPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PriorityClassPropertyViewState ReduceFetchKubernetesPriorityClassPropertyAction(PriorityClassPropertyViewState state, FetchKubernetesPriorityClassPropertyAction action)
        => (FetchStateBegin(state, action) as PriorityClassPropertyViewState)!;

    [ReducerMethod]
    public static PriorityClassPropertyViewState ReduceFetchKubernetesPriorityClassPropertyActionResult(PriorityClassPropertyViewState state, FetchKubernetesPriorityClassPropertyActionResult action)
        => (FetchStateResult(state, action) as PriorityClassPropertyViewState)!;
}

internal class PriorityClassPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public PriorityClassPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesPriorityClassPropertyAction action, IDispatcher dispatcher)
    {
        var priorityClass = await _viewStateHelper.GetPriorityClass(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (priorityClass != null)
        {
            var properties = new PriorityClassPropertyViewModel()
            {
                Created = priorityClass.Metadata.CreationTimestamp,
                Name = priorityClass.Metadata.Name,
                Tab = action.Tab,
                Uid = priorityClass.Metadata.Uid,
                PriorityClass = priorityClass
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesPriorityClassPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}