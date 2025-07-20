using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record ConfigMapViewState : GenericFeatureState<ConfigMapViewModel>;
public record FetchKubernetesConfigMapAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ConfigMapViewModel>(Tab, CancellationToken);
public record FetchKubernetesConfigMapActionResult(Tab Tab, IEnumerable<ConfigMapViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ConfigMapViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ConfigMapViewState ReduceFetchKubernetesConfigMapAction(ConfigMapViewState state, FetchKubernetesConfigMapAction action)
        => FetchStateBegin(state, action) as ConfigMapViewState;

    [ReducerMethod]
    public static ConfigMapViewState ReduceFetchKubernetesConfigMapActionResult(ConfigMapViewState state, FetchKubernetesConfigMapActionResult action)
        => FetchStateResult(state, action) as ConfigMapViewState;
}

public class ConfigMapViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public ConfigMapViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesConfigMapAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ConfigMapViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListConfigMapForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new ConfigMapViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Data?.Select(y => y.Key).ToArray() ?? []
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ConfigMap, items);
        dispatcher.Dispatch(new FetchKubernetesConfigMapActionResult(action.Tab, items ?? []));
    }
}
