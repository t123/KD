using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
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
    private readonly IViewStateHelper _viewStateHelper;

    public IngressClassConfigurationPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesIngressClassPropertyAction action, IDispatcher dispatcher)
    {
        var ingressClass = await _viewStateHelper.GetIngressClass(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (ingressClass != null)
        {
            var properties = new IngressClassPropertyViewModel
            {
                Created = ingressClass.Metadata.CreationTimestamp,
                Name = ingressClass.Metadata.Name,
                Tab = action.Tab,
                Uid = ingressClass.Metadata.Uid,
                IngressClass = ingressClass
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesIngressClassPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}