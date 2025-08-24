using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record PriorityClassPropertyViewState : GenericPropertyFeatureState<PriorityClassPropertyViewModel>;
public record FetchKubernetesPriorityClassPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<PriorityClassPropertyViewModel>(Tab, CancellationToken);
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
    private readonly IIndexManager _indexManager;

    public PriorityClassPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesPriorityClassPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new PriorityClassPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesPriorityClassPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}