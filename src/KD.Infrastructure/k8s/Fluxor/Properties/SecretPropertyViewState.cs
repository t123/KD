using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record SecretPropertyViewState : GenericPropertyFeatureState<SecretPropertyViewModel>;
public record FetchKubernetesSecretPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<SecretPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesSecretPropertyActionResult(TabModel Tab, SecretPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<SecretPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static SecretPropertyViewState ReduceFetchKubernetesSecretPropertyAction(SecretPropertyViewState state, FetchKubernetesSecretPropertyAction action)
        => (FetchStateBegin(state, action) as SecretPropertyViewState)!;

    [ReducerMethod]
    public static SecretPropertyViewState ReduceFetchKubernetesSecretPropertyActionResult(SecretPropertyViewState state, FetchKubernetesSecretPropertyActionResult action)
        => (FetchStateResult(state, action) as SecretPropertyViewState)!;
}

internal class SecretPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public SecretPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesSecretPropertyAction action, IDispatcher dispatcher)
    {
        var secret = await _viewStateHelper.GetSecret(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (secret != null)
        {
            var properties = new SecretPropertyViewModel()
            {
                Created = secret.Metadata.CreationTimestamp,
                Name = secret.Metadata.Name,
                Tab = action.Tab,
                Uid = secret.Metadata.Uid,
                Secret = secret
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesSecretPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}