using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record MutatingWebhookConfigurationViewState : GenericFeatureState<MutatingWebhookConfigurationViewModel>;
public record FetchKubernetesMutatingWebhookConfigurationAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<MutatingWebhookConfigurationViewModel>(Tab, CancellationToken);
public record FetchKubernetesMutatingWebhookConfigurationActionResult(Tab Tab, IEnumerable<MutatingWebhookConfigurationViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<MutatingWebhookConfigurationViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static MutatingWebhookConfigurationViewState ReduceFetchKubernetesMutatingWebhookConfigurationAction(MutatingWebhookConfigurationViewState state, FetchKubernetesMutatingWebhookConfigurationAction action)
        => FetchStateBegin(state, action) as MutatingWebhookConfigurationViewState;

    [ReducerMethod]
    public static MutatingWebhookConfigurationViewState ReduceFetchKubernetesMutatingWebhookConfigurationActionResult(MutatingWebhookConfigurationViewState state, FetchKubernetesMutatingWebhookConfigurationActionResult action)
        => FetchStateResult(state, action) as MutatingWebhookConfigurationViewState;
}

public class MutatingWebhookConfigurationViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public MutatingWebhookConfigurationViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesMutatingWebhookConfigurationAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<MutatingWebhookConfigurationViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListMutatingWebhookConfigurationAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new MutatingWebhookConfigurationViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.MutatingWebhookConfiguration, items);
        dispatcher.Dispatch(new FetchKubernetesMutatingWebhookConfigurationActionResult(action.Tab, items ?? []));
    }
}
