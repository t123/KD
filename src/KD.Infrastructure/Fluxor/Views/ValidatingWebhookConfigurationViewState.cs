using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record ValidatingWebhookConfigurationViewState : GenericFeatureState<ValidatingWebhookConfigurationViewModel>;
public record FetchKubernetesValidatingWebhookConfigurationAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ValidatingWebhookConfigurationViewModel>(Tab, CancellationToken);
public record FetchKubernetesValidatingWebhookConfigurationActionResult(Tab Tab, IEnumerable<ValidatingWebhookConfigurationViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ValidatingWebhookConfigurationViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ValidatingWebhookConfigurationViewState ReduceFetchKubernetesValidatingWebhookConfigurationAction(ValidatingWebhookConfigurationViewState state, FetchKubernetesValidatingWebhookConfigurationAction action)
        => FetchStateBegin(state, action) as ValidatingWebhookConfigurationViewState;

    [ReducerMethod]
    public static ValidatingWebhookConfigurationViewState ReduceFetchKubernetesValidatingWebhookConfigurationActionResult(ValidatingWebhookConfigurationViewState state, FetchKubernetesValidatingWebhookConfigurationActionResult action)
        => FetchStateResult(state, action) as ValidatingWebhookConfigurationViewState;
}

public class ValidatingWebhookConfigurationViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public ValidatingWebhookConfigurationViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesValidatingWebhookConfigurationAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ValidatingWebhookConfigurationViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListValidatingWebhookConfigurationAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new ValidatingWebhookConfigurationViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ValidatingWebhookConfiguration, items);
        dispatcher.Dispatch(new FetchKubernetesValidatingWebhookConfigurationActionResult(action.Tab, items ?? []));
    }
}
