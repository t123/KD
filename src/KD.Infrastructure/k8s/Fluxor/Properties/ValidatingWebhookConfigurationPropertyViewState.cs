using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record ValidatingWebhookConfigurationPropertyViewState : GenericPropertyFeatureState<ValidatingWebhookConfigurationPropertyViewModel>;
public record FetchKubernetesValidatingWebhookConfigurationPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<ValidatingWebhookConfigurationPropertyViewModel>(Tab, CancellationToken);
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
    private readonly IIndexManager _indexManager;

    public ValidatingWebhookConfigurationPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesValidatingWebhookConfigurationPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new ValidatingWebhookConfigurationPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesValidatingWebhookConfigurationPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}