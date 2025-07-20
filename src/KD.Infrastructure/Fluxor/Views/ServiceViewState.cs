using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record ServiceViewState : GenericFeatureState<ServiceViewModel>;
public record FetchKubernetesServiceAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<ServiceViewModel>(Tab, CancellationToken);
public record FetchKubernetesServiceActionResult(Tab Tab, IEnumerable<ServiceViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<ServiceViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static ServiceViewState ReduceFetchKubernetesServiceAction(ServiceViewState state, FetchKubernetesServiceAction action)
        => FetchStateBegin(state, action) as ServiceViewState;

    [ReducerMethod]
    public static ServiceViewState ReduceFetchKubernetesServiceActionResult(ServiceViewState state, FetchKubernetesServiceActionResult action)
        => FetchStateResult(state, action) as ServiceViewState;
}

public class ServiceViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public ServiceViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesServiceAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<ServiceViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListServiceForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x =>
                {
                    var internalEndpoints = new List<string>();
                    foreach (var endpoint in x.Spec.Ports)
                    {
                        string ns = (x.Namespace() ?? "") == "default" ? "" : $".{x.Namespace}";
                        string name = $"{x.Name}{ns}";

                        internalEndpoints.Add($"{endpoint.Name}:{endpoint.Port} {endpoint.Protocol}");
                        internalEndpoints.Add($"{endpoint.Name}:{endpoint.NodePort} {endpoint.Protocol}");
                    }

                    var model = new ServiceViewModel(x.Uid(), x.Name(), x.Namespace(), x.Metadata.CreationTimestamp, x.Spec.Type, x.Spec.ClusterIP, internalEndpoints.ToArray(), []);
                    return model;
                })
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Service, items);
        dispatcher.Dispatch(new FetchKubernetesServiceActionResult(action.Tab, items ?? []));
    }
}