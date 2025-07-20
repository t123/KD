using Examine;
using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record DeploymentViewState : GenericFeatureState<DeploymentViewModel>;
public record FetchKubernetesDeploymentsAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<DeploymentViewModel>(Tab, CancellationToken);
public record FetchKubernetesDeploymentsActionResult(Tab Tab, IEnumerable<DeploymentViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<DeploymentViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static DeploymentViewState ReduceFetchKubernetesDeploymentAction(DeploymentViewState state, FetchKubernetesDeploymentsAction action)
        => FetchStateBegin(state, action) as DeploymentViewState;

    [ReducerMethod]
    public static DeploymentViewState ReduceFetchKubernetesDeploymentActionResult(DeploymentViewState state, FetchKubernetesDeploymentsActionResult action)
        => FetchStateResult(state, action) as DeploymentViewState;
}

public class DeploymentViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public DeploymentViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesPodsAction(FetchKubernetesDeploymentsAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<DeploymentViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListDeploymentForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new DeploymentViewModel(x.Uid(), x.Name(), x.Namespace(), x.Status.Replicas, x.Status.ReadyReplicas, x.Status.AvailableReplicas, x.Metadata.CreationTimestamp))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        var result = new FetchKubernetesDeploymentsActionResult(action.Tab, items ?? []);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Deployment, items);
        dispatcher.Dispatch(result);
    }
}