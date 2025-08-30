using Fluxor;
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
    private readonly IIndexManager _indexManager;

    public PodDisruptionBudgetPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesPodDisruptionBudgetPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new PodDisruptionBudgetPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesPodDisruptionBudgetPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}