using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record IngressClassViewState : GenericFeatureState<IngressClassViewModel>;
public record FetchKubernetesIngressClassAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<IngressClassViewModel>(Tab, CancellationToken);
public record FetchKubernetesIngressClassActionResult(TabModel Tab, IEnumerable<IngressClassViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<IngressClassViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static IngressClassViewState ReduceFetchKubernetesIngressClassAction(IngressClassViewState state, FetchKubernetesIngressClassAction action)
        => FetchStateBegin(state, action) as IngressClassViewState;

    [ReducerMethod]
    public static IngressClassViewState ReduceFetchKubernetesIngressClassActionResult(IngressClassViewState state, FetchKubernetesIngressClassActionResult action)
        => FetchStateResult(state, action) as IngressClassViewState;
}

internal class IngressClassViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public IngressClassViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesIngressClassAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<IngressClassViewModel>? items = await _viewStateHelper.GetIngressClasses(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.IngressClass, items);
        dispatcher.Dispatch(new FetchKubernetesIngressClassActionResult(action.Tab, items ?? []));
    }
}
