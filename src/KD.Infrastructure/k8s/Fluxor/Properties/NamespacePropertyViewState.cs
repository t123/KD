using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record NamespacePropertyViewState : GenericPropertyFeatureState<NamespacePropertyViewModel>;
public record FetchKubernetesNamespacePropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<NamespacePropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesNamespacePropertyActionResult(TabModel Tab, NamespacePropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<NamespacePropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static NamespacePropertyViewState ReduceFetchKubernetesNamespacePropertyAction(NamespacePropertyViewState state, FetchKubernetesNamespacePropertyAction action)
        => (FetchStateBegin(state, action) as NamespacePropertyViewState)!;

    [ReducerMethod]
    public static NamespacePropertyViewState ReduceFetchKubernetesNamespacePropertyActionResult(NamespacePropertyViewState state, FetchKubernetesNamespacePropertyActionResult action)
        => (FetchStateResult(state, action) as NamespacePropertyViewState)!;
}

internal class NamespacePropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public NamespacePropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesNamespacePropertyAction action, IDispatcher dispatcher)
    {
        var ns = await _viewStateHelper.GetNamespace(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (ns != null)
        {
            var properties = new NamespacePropertyViewModel()
            {
                Created = ns.Metadata.CreationTimestamp,
                Name = ns.Metadata.Name,
                Tab = action.Tab,
                Uid = ns.Metadata.Uid,
                Namespace = ns
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesNamespacePropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}