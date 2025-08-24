using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record MutatingWebhookConfigurationViewState : GenericFeatureState<MutatingWebhookConfigurationViewModel>;
public record FetchKubernetesMutatingWebhookConfigurationAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<MutatingWebhookConfigurationViewModel>(Tab, CancellationToken);
public record FetchKubernetesMutatingWebhookConfigurationActionResult(TabModel Tab, IEnumerable<MutatingWebhookConfigurationViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<MutatingWebhookConfigurationViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static MutatingWebhookConfigurationViewState ReduceFetchKubernetesMutatingWebhookConfigurationAction(MutatingWebhookConfigurationViewState state, FetchKubernetesMutatingWebhookConfigurationAction action)
        => FetchStateBegin(state, action) as MutatingWebhookConfigurationViewState;

    [ReducerMethod]
    public static MutatingWebhookConfigurationViewState ReduceFetchKubernetesMutatingWebhookConfigurationActionResult(MutatingWebhookConfigurationViewState state, FetchKubernetesMutatingWebhookConfigurationActionResult action)
        => FetchStateResult(state, action) as MutatingWebhookConfigurationViewState;
}

internal class MutatingWebhookConfigurationViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public MutatingWebhookConfigurationViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesMutatingWebhookConfigurationAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<MutatingWebhookConfigurationViewModel>? items = await _viewStateHelper.GetMutatingWebhookConfigurations(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken); ;
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.MutatingWebhookConfiguration, items);
        dispatcher.Dispatch(new FetchKubernetesMutatingWebhookConfigurationActionResult(action.Tab, items ?? []));
    }
}
