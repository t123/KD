using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record StatefulSetPropertyViewState : GenericPropertyFeatureState<StatefulSetPropertyViewModel>;
public record FetchKubernetesStatefulSetPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<StatefulSetPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesStatefulSetPropertyActionResult(TabModel Tab, StatefulSetPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<StatefulSetPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static StatefulSetPropertyViewState ReduceFetchKubernetesStatefulSetPropertyAction(StatefulSetPropertyViewState state, FetchKubernetesStatefulSetPropertyAction action)
        => (FetchStateBegin(state, action) as StatefulSetPropertyViewState)!;

    [ReducerMethod]
    public static StatefulSetPropertyViewState ReduceFetchKubernetesStatefulSetPropertyActionResult(StatefulSetPropertyViewState state, FetchKubernetesStatefulSetPropertyActionResult action)
        => (FetchStateResult(state, action) as StatefulSetPropertyViewState)!;
}

internal class StatefulSetPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public StatefulSetPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesStatefulSetPropertyAction action, IDispatcher dispatcher)
    {
        var statefulSet = await _viewStateHelper.GetStatefulSet(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (statefulSet != null)
        {
            var properties = new StatefulSetPropertyViewModel()
            {
                Created = statefulSet.Metadata.CreationTimestamp,
                Name = statefulSet.Metadata.Name,
                Tab = action.Tab,
                Uid = statefulSet.Metadata.Uid,
                StatefulSet = statefulSet
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesStatefulSetPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}