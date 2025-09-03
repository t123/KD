using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record SecretViewState : GenericFeatureState<SecretViewModel>;
public record FetchKubernetesSecretAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<SecretViewModel>(Tab, CancellationToken);
public record FetchKubernetesSecretActionResult(TabModel Tab, IEnumerable<SecretViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<SecretViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static SecretViewState ReduceFetchKubernetesSecretAction(SecretViewState state, FetchKubernetesSecretAction action)
        => FetchStateBegin(state, action) as SecretViewState;

    [ReducerMethod]
    public static SecretViewState ReduceFetchKubernetesSecretActionResult(SecretViewState state, FetchKubernetesSecretActionResult action)
        => FetchStateResult(state, action) as SecretViewState;
}

internal class SecretViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public SecretViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesSecretAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<SecretViewModel>? items = await _viewStateHelper.GetSecrets(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Secret, items);
        dispatcher.Dispatch(new FetchKubernetesSecretActionResult(action.Tab, items ?? []));
    }
}
