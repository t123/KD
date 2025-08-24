using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record StatefulSetPropertyViewState : GenericPropertyFeatureState<StatefulSetPropertyViewModel>;
public record FetchKubernetesStatefulSetPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<StatefulSetPropertyViewModel>(Tab, CancellationToken);
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
    private readonly IIndexManager _indexManager;

    public StatefulSetPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesStatefulSetPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new StatefulSetPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesStatefulSetPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}