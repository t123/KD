using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record IngressClassPropertyViewState : GenericPropertyFeatureState<IngressClassPropertyViewModel>;
public record FetchKubernetesIngressClassPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<IngressClassPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesIngressClassPropertyActionResult(TabModel Tab, IngressClassPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<IngressClassPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static IngressClassPropertyViewState ReduceFetchKubernetesIngressClassPropertyAction(IngressClassPropertyViewState state, FetchKubernetesIngressClassPropertyAction action)
        => (FetchStateBegin(state, action) as IngressClassPropertyViewState)!;

    [ReducerMethod]
    public static IngressClassPropertyViewState ReduceFetchKubernetesIngressClassPropertyActionResult(IngressClassPropertyViewState state, FetchKubernetesIngressClassPropertyActionResult action)
        => (FetchStateResult(state, action) as IngressClassPropertyViewState)!;
}

internal class IngressClassConfigurationPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public IngressClassConfigurationPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesIngressClassPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new IngressClassPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesIngressClassPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}