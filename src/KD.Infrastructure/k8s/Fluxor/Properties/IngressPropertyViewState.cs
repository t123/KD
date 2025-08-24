using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record IngressPropertyViewState : GenericPropertyFeatureState<IngressPropertyViewModel>;
public record FetchKubernetesIngressPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<IngressPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesIngressPropertyActionResult(TabModel Tab, IngressPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<IngressPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static IngressPropertyViewState ReduceFetchKubernetesIngressPropertyAction(IngressPropertyViewState state, FetchKubernetesIngressPropertyAction action)
        => (FetchStateBegin(state, action) as IngressPropertyViewState)!;

    [ReducerMethod]
    public static IngressPropertyViewState ReduceFetchKubernetesIngressPropertyActionResult(IngressPropertyViewState state, FetchKubernetesIngressPropertyActionResult action)
        => (FetchStateResult(state, action) as IngressPropertyViewState)!;
}

internal class IngressPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public IngressPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesIngressPropertyAction action, IDispatcher dispatcher)
    {
        var ingress = await _viewStateHelper.GetIngress(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (ingress != null)
        {
            var properties = new IngressPropertyViewModel()
            {
                Created = ingress.Metadata.CreationTimestamp,
                Name = ingress.Metadata.Name,
                Tab = action.Tab,
                Uid = ingress.Metadata.Uid,
                Ingress = ingress
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesIngressPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}