using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record RuntimeClassViewState : GenericFeatureState<RuntimeClassViewModel>;
public record FetchKubernetesRuntimeClassAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<RuntimeClassViewModel>(Tab, CancellationToken);
public record FetchKubernetesRuntimeClassActionResult(TabModel Tab, IEnumerable<RuntimeClassViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<RuntimeClassViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RuntimeClassViewState ReduceFetchKubernetesRuntimeClassAction(RuntimeClassViewState state, FetchKubernetesRuntimeClassAction action)
        => FetchStateBegin(state, action) as RuntimeClassViewState;

    [ReducerMethod]
    public static RuntimeClassViewState ReduceFetchKubernetesRuntimeClassActionResult(RuntimeClassViewState state, FetchKubernetesRuntimeClassActionResult action)
        => FetchStateResult(state, action) as RuntimeClassViewState;
}

internal class RuntimeClassViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public RuntimeClassViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesRuntimeClassAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<RuntimeClassViewModel>? items = await _viewStateHelper.GetRuntimes(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.RuntimeClass, items);
        dispatcher.Dispatch(new FetchKubernetesRuntimeClassActionResult(action.Tab, items ?? []));
    }
}
