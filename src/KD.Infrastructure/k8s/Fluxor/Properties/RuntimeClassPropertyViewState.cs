using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record RuntimeClassPropertyViewState : GenericPropertyFeatureState<RuntimeClassPropertyViewModel>;
public record FetchKubernetesRuntimeClassPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<RuntimeClassPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesRuntimeClassPropertyActionResult(TabModel Tab, RuntimeClassPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<RuntimeClassPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RuntimeClassPropertyViewState ReduceFetchKubernetesRuntimeClassPropertyAction(RuntimeClassPropertyViewState state, FetchKubernetesRuntimeClassPropertyAction action)
        => (FetchStateBegin(state, action) as RuntimeClassPropertyViewState)!;

    [ReducerMethod]
    public static RuntimeClassPropertyViewState ReduceFetchKubernetesRuntimeClassPropertyActionResult(RuntimeClassPropertyViewState state, FetchKubernetesRuntimeClassPropertyActionResult action)
        => (FetchStateResult(state, action) as RuntimeClassPropertyViewState)!;
}

internal class RuntimeClassPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public RuntimeClassPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesRuntimeClassPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new RuntimeClassPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesRuntimeClassPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}