using Fluxor;
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
    private readonly IIndexManager _indexManager;

    public SecretPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesSecretPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new SecretPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesSecretPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}