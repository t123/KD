using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record ConfigMapViewState : GenericFeatureState<ConfigMapViewModel>;
public record FetchKubernetesConfigMapAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ConfigMapViewModel>(Tab, CancellationToken);
public record FetchKubernetesConfigMapActionResult(TabModel Tab, IEnumerable<ConfigMapViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ConfigMapViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ConfigMapViewState ReduceFetchKubernetesConfigMapAction(ConfigMapViewState state, FetchKubernetesConfigMapAction action)
        => FetchStateBegin(state, action) as ConfigMapViewState;

    [ReducerMethod]
    public static ConfigMapViewState ReduceFetchKubernetesConfigMapActionResult(ConfigMapViewState state, FetchKubernetesConfigMapActionResult action)
        => FetchStateResult(state, action) as ConfigMapViewState;
}

internal class ConfigMapViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public ConfigMapViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesConfigMapAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        var items = await _viewStateHelper.GetConfigMaps(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ConfigMap, items);
        dispatcher.Dispatch(new FetchKubernetesConfigMapActionResult(action.Tab, items ?? []));
    }
}
