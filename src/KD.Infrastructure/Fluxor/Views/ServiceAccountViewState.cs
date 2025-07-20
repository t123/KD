using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record ServiceAccountViewState : GenericFeatureState<ServiceAccountViewModel>;
public record FetchKubernetesServiceAccountAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ServiceAccountViewModel>(Tab, CancellationToken);
public record FetchKubernetesServiceAccountActionResult(Tab Tab, IEnumerable<ServiceAccountViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ServiceAccountViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ServiceAccountViewState ReduceFetchKubernetesServiceAccountAction(ServiceAccountViewState state, FetchKubernetesServiceAccountAction action)
        => FetchStateBegin(state, action) as ServiceAccountViewState;

    [ReducerMethod]
    public static ServiceAccountViewState ReduceFetchKubernetesServiceAccountActionResult(ServiceAccountViewState state, FetchKubernetesServiceAccountActionResult action)
        => FetchStateResult(state, action) as ServiceAccountViewState;
}

public class ServiceAccountViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public ServiceAccountViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesServiceAccountAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ServiceAccountViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListServiceAccountForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new ServiceAccountViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.ServiceAccount, items);
        dispatcher.Dispatch(new FetchKubernetesServiceAccountActionResult(action.Tab, items ?? []));
    }
}
