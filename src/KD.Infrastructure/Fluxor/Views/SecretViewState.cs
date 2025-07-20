using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record SecretViewState : GenericFeatureState<SecretViewModel>;
public record FetchKubernetesSecretAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<SecretViewModel>(Tab, CancellationToken);
public record FetchKubernetesSecretActionResult(Tab Tab, IEnumerable<SecretViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<SecretViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static SecretViewState ReduceFetchKubernetesSecretAction(SecretViewState state, FetchKubernetesSecretAction action)
        => FetchStateBegin(state, action) as SecretViewState;

    [ReducerMethod]
    public static SecretViewState ReduceFetchKubernetesSecretActionResult(SecretViewState state, FetchKubernetesSecretActionResult action)
        => FetchStateResult(state, action) as SecretViewState;
}

public class SecretViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public SecretViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesSecretAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<SecretViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListSecretForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new SecretViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Data.Keys.ToArray()
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Secret, items);
        dispatcher.Dispatch(new FetchKubernetesSecretActionResult(action.Tab, items ?? []));
    }
}
