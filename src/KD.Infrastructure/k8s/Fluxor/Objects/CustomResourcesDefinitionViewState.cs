using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record CustomResourcesDefinitionViewState : GenericFeatureState<CustomResourcesDefinitionViewModel>;
public record FetchKubernetesCustomResourcesDefinitionAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<CustomResourcesDefinitionViewModel>(Tab, CancellationToken);
public record FetchKubernetesCustomResourcesDefinitionActionResult(TabModel Tab, IEnumerable<CustomResourcesDefinitionViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<CustomResourcesDefinitionViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static CustomResourcesDefinitionViewState ReduceFetchKubernetesCustomResourcesDefinitionAction(CustomResourcesDefinitionViewState state, FetchKubernetesCustomResourcesDefinitionAction action)
        => FetchStateBegin(state, action) as CustomResourcesDefinitionViewState;

    [ReducerMethod]
    public static CustomResourcesDefinitionViewState ReduceFetchKubernetesCustomResourcesDefinitionActionResult(CustomResourcesDefinitionViewState state, FetchKubernetesCustomResourcesDefinitionActionResult action)
        => FetchStateResult(state, action) as CustomResourcesDefinitionViewState;
}

internal class CustomResourcesDefinitionViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public CustomResourcesDefinitionViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesCustomResourcesDefinitionAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<CustomResourcesDefinitionViewModel>? items = await _viewStateHelper.GetCustomResources(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken); ;
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.CustomResourcesDefinition, items);
        dispatcher.Dispatch(new FetchKubernetesCustomResourcesDefinitionActionResult(action.Tab, items ?? []));
    }
}
