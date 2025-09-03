using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record MutatingWebhookConfigurationPropertyViewState : GenericPropertyFeatureState<MutatingWebhookConfigurationPropertyViewModel>;
public record FetchKubernetesMutatingWebhookConfigurationPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<MutatingWebhookConfigurationPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesMutatingWebhookConfigurationPropertyActionResult(TabModel Tab, MutatingWebhookConfigurationPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<MutatingWebhookConfigurationPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static MutatingWebhookConfigurationPropertyViewState ReduceFetchKubernetesMutatingWebhookConfigurationPropertyAction(MutatingWebhookConfigurationPropertyViewState state, FetchKubernetesMutatingWebhookConfigurationPropertyAction action)
        => (FetchStateBegin(state, action) as MutatingWebhookConfigurationPropertyViewState)!;

    [ReducerMethod]
    public static MutatingWebhookConfigurationPropertyViewState ReduceFetchKubernetesMutatingWebhookConfigurationPropertyActionResult(MutatingWebhookConfigurationPropertyViewState state, FetchKubernetesMutatingWebhookConfigurationPropertyActionResult action)
        => (FetchStateResult(state, action) as MutatingWebhookConfigurationPropertyViewState)!;
}

internal class MutatingWebhookConfigurationPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public MutatingWebhookConfigurationPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesMutatingWebhookConfigurationPropertyAction action, IDispatcher dispatcher)
    {
        var mwh = await _viewStateHelper.GetMutatingWebhookConfiguration(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (mwh != null)
        {
            var properties = new MutatingWebhookConfigurationPropertyViewModel()
            {
                Created = mwh.Metadata.CreationTimestamp,
                Name = mwh.Metadata.Name,
                Tab = action.Tab,
                Uid = mwh.Metadata.Uid,
                MutatingWebhookConfiguration = mwh
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesMutatingWebhookConfigurationPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}