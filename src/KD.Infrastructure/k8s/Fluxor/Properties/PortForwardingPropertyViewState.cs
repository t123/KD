using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record PortForwardingPropertyViewState : GenericPropertyFeatureState<PortForwardingPropertyViewModel>;
public record FetchKubernetesPortForwardingPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<PortForwardingPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesPortForwardingPropertyActionResult(TabModel Tab, PortForwardingPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<PortForwardingPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PortForwardingPropertyViewState ReduceFetchKubernetesPortForwardingPropertyAction(PortForwardingPropertyViewState state, FetchKubernetesPortForwardingPropertyAction action)
        => (FetchStateBegin(state, action) as PortForwardingPropertyViewState)!;

    [ReducerMethod]
    public static PortForwardingPropertyViewState ReduceFetchKubernetesPortForwardingPropertyActionResult(PortForwardingPropertyViewState state, FetchKubernetesPortForwardingPropertyActionResult action)
        => (FetchStateResult(state, action) as PortForwardingPropertyViewState)!;
}

internal class PortForwardingPropertyViewStateEffects
{
    private readonly IIndexManager _indexManager;

    public PortForwardingPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesPortForwardingPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new PortForwardingPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesPortForwardingPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}