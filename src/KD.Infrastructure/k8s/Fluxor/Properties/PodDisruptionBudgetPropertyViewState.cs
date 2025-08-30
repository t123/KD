using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record PodDisruptionBudgetPropertyViewState : GenericPropertyFeatureState<PodDisruptionBudgetPropertyViewModel>;
public record FetchKubernetesPodDisruptionBudgetPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<PodDisruptionBudgetPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesPodDisruptionBudgetPropertyActionResult(TabModel Tab, PodDisruptionBudgetPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<PodDisruptionBudgetPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PodDisruptionBudgetPropertyViewState ReduceFetchKubernetesPodDisruptionBudgetPropertyAction(PodDisruptionBudgetPropertyViewState state, FetchKubernetesPodDisruptionBudgetPropertyAction action)
        => (FetchStateBegin(state, action) as PodDisruptionBudgetPropertyViewState)!;

    [ReducerMethod]
    public static PodDisruptionBudgetPropertyViewState ReduceFetchKubernetesPodDisruptionBudgetPropertyActionResult(PodDisruptionBudgetPropertyViewState state, FetchKubernetesPodDisruptionBudgetPropertyActionResult action)
        => (FetchStateResult(state, action) as PodDisruptionBudgetPropertyViewState)!;
}

internal class PodDisruptionBudgetPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public PodDisruptionBudgetPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesPodDisruptionBudgetPropertyAction action, IDispatcher dispatcher)
    {
        var pdb = await _viewStateHelper.GetPodDisruptionBudget(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (pdb != null)
        {
            var properties = new PodDisruptionBudgetPropertyViewModel()
            {
                Created = pdb.Metadata.CreationTimestamp,
                Name = pdb.Metadata.Name,
                Tab = action.Tab,
                Uid = pdb.Metadata.Uid,
                PodDisruptionBudget = pdb
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesPodDisruptionBudgetPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}