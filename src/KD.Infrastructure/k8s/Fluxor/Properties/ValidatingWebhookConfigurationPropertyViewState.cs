using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ValidatingWebhookConfigurationPropertyViewState : GenericPropertyFeatureState<ValidatingWebhookConfigurationPropertyViewModel>;
public record FetchKubernetesValidatingWebhookConfigurationPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ValidatingWebhookConfigurationPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesValidatingWebhookConfigurationPropertyActionResult(TabModel Tab, ValidatingWebhookConfigurationPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<ValidatingWebhookConfigurationPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ValidatingWebhookConfigurationPropertyViewState ReduceFetchKubernetesValidatingWebhookConfigurationPropertyAction(ValidatingWebhookConfigurationPropertyViewState state, FetchKubernetesValidatingWebhookConfigurationPropertyAction action)
        => (FetchStateBegin(state, action) as ValidatingWebhookConfigurationPropertyViewState)!;

    [ReducerMethod]
    public static ValidatingWebhookConfigurationPropertyViewState ReduceFetchKubernetesValidatingWebhookConfigurationPropertyActionResult(ValidatingWebhookConfigurationPropertyViewState state, FetchKubernetesValidatingWebhookConfigurationPropertyActionResult action)
        => (FetchStateResult(state, action) as ValidatingWebhookConfigurationPropertyViewState)!;
}

internal class ValidatingWebhookConfigurationPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public ValidatingWebhookConfigurationPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesValidatingWebhookConfigurationPropertyAction action, IDispatcher dispatcher)
    {
        var vwc = await _viewStateHelper.GetValidatingWebhookConfiguration(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (vwc != null)
        {
            var properties = new ValidatingWebhookConfigurationPropertyViewModel()
            {
                Created = vwc.Metadata.CreationTimestamp,
                Name = vwc.Metadata.Name,
                Tab = action.Tab,
                Uid = vwc.Metadata.Uid,
                ValidatingWebhookConfiguration = vwc
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesValidatingWebhookConfigurationPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}