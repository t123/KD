using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record ResourceQuotaViewState : GenericFeatureState<ResourceQuotaViewModel>;
public record FetchKubernetesResourceQuotaAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ResourceQuotaViewModel>(Tab, CancellationToken);
public record FetchKubernetesResourceQuotaActionResult(Tab Tab, IEnumerable<ResourceQuotaViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ResourceQuotaViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ResourceQuotaViewState ReduceFetchKubernetesResourceQuotaAction(ResourceQuotaViewState state, FetchKubernetesResourceQuotaAction action)
        => FetchStateBegin(state, action) as ResourceQuotaViewState;

    [ReducerMethod]
    public static ResourceQuotaViewState ReduceFetchKubernetesResourceQuotaActionResult(ResourceQuotaViewState state, FetchKubernetesResourceQuotaActionResult action)
        => FetchStateResult(state, action) as ResourceQuotaViewState;
}

public class ResourceQuotaViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public ResourceQuotaViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesResourceQuotaAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ResourceQuotaViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListResourceQuotaForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new ResourceQuotaViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ResourceQuota, items);
        dispatcher.Dispatch(new FetchKubernetesResourceQuotaActionResult(action.Tab, items ?? []));
    }
}
