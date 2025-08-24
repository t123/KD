using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record ValidatingWebhookConfigurationViewState : GenericFeatureState<ValidatingWebhookConfigurationViewModel>;
public record FetchKubernetesValidatingWebhookConfigurationAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ValidatingWebhookConfigurationViewModel>(Tab, CancellationToken);
public record FetchKubernetesValidatingWebhookConfigurationActionResult(TabModel Tab, IEnumerable<ValidatingWebhookConfigurationViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ValidatingWebhookConfigurationViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ValidatingWebhookConfigurationViewState ReduceFetchKubernetesValidatingWebhookConfigurationAction(ValidatingWebhookConfigurationViewState state, FetchKubernetesValidatingWebhookConfigurationAction action)
        => FetchStateBegin(state, action) as ValidatingWebhookConfigurationViewState;

    [ReducerMethod]
    public static ValidatingWebhookConfigurationViewState ReduceFetchKubernetesValidatingWebhookConfigurationActionResult(ValidatingWebhookConfigurationViewState state, FetchKubernetesValidatingWebhookConfigurationActionResult action)
        => FetchStateResult(state, action) as ValidatingWebhookConfigurationViewState;
}

internal class ValidatingWebhookConfigurationViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public ValidatingWebhookConfigurationViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesValidatingWebhookConfigurationAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ValidatingWebhookConfigurationViewModel>? items = await _viewStateHelper.GetValidatingWebhookConfigurations(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ValidatingWebhookConfiguration, items);
        dispatcher.Dispatch(new FetchKubernetesValidatingWebhookConfigurationActionResult(action.Tab, items ?? []));
    }
}
