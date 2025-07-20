using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record IngressViewState : GenericFeatureState<IngressViewModel>;
public record FetchKubernetesIngressAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<IngressViewModel>(Tab, CancellationToken);
public record FetchKubernetesIngressActionResult(Tab Tab, IEnumerable<IngressViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<IngressViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static IngressViewState ReduceFetchKubernetesIngressAction(IngressViewState state, FetchKubernetesIngressAction action)
        => FetchStateBegin(state, action) as IngressViewState;

    [ReducerMethod]
    public static IngressViewState ReduceFetchKubernetesIngressActionResult(IngressViewState state, FetchKubernetesIngressActionResult action)
        => FetchStateResult(state, action) as IngressViewState;
}

public class IngressViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public IngressViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesIngressAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<IngressViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.NetworkingV1.ListIngressForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items.Select(x =>
                {
                    List<string> rules = new List<string>();

                    foreach (var rule in x.Spec.Rules)
                    {
                        string host = rule.Host;
                        string[] paths = rule.Http.Paths.Select(path => host + path.Path).ToArray();
                        rules.AddRange(paths);
                    }

                    var model = new IngressViewModel(
                        x.Uid(),
                        x.Name(),
                        x.Namespace(),
                        x.Metadata.CreationTimestamp,
                        rules.ToArray(),
                        x.Status.LoadBalancer.Ingress.Select(x => x.Ip).ToArray()
                    );
                    return model;
                })
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Ingress, items);
        dispatcher.Dispatch(new FetchKubernetesIngressActionResult(action.Tab, items ?? []));
    }
}

