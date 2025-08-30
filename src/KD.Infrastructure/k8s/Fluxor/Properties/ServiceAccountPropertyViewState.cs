using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ServiceAccountPropertyViewState : GenericPropertyFeatureState<ServiceAccountPropertyViewModel>;
public record FetchKubernetesServiceAccountPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ServiceAccountPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesServiceAccountPropertyActionResult(TabModel Tab, ServiceAccountPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ServiceAccountPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ServiceAccountPropertyViewState ReduceFetchKubernetesServiceAccountPropertyAction(ServiceAccountPropertyViewState state, FetchKubernetesServiceAccountPropertyAction action)
        => (FetchStateBegin(state, action) as ServiceAccountPropertyViewState)!;

    [ReducerMethod]
    public static ServiceAccountPropertyViewState ReduceFetchKubernetesServiceAccountPropertyActionResult(ServiceAccountPropertyViewState state, FetchKubernetesServiceAccountPropertyActionResult action)
        => (FetchStateResult(state, action) as ServiceAccountPropertyViewState)!;
}

internal class ServiceAccountPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public ServiceAccountPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesServiceAccountPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new ServiceAccountPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesServiceAccountPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}