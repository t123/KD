using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

[FeatureState]
public record DeploymentViewState : GenericFeatureState<DeploymentViewModel>;
public record FetchKubernetesDeploymentsAction(TabModel Tab, string[] SelectedNamespaces, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<DeploymentViewModel>(Tab, CancellationToken);
public record FetchKubernetesDeploymentsActionResult(TabModel Tab, IEnumerable<DeploymentViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<DeploymentViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static DeploymentViewState ReduceFetchKubernetesDeploymentAction(DeploymentViewState state, FetchKubernetesDeploymentsAction action)
        => FetchStateBegin(state, action) as DeploymentViewState;

    [ReducerMethod]
    public static DeploymentViewState ReduceFetchKubernetesDeploymentActionResult(DeploymentViewState state, FetchKubernetesDeploymentsActionResult action)
        => FetchStateResult(state, action) as DeploymentViewState;
}

internal class DeploymentViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;
    private readonly IIndexManager _indexManager;

    public DeploymentViewStateEffects(IViewStateHelper viewStateHelper, IIndexManager indexManager)
    {
        _viewStateHelper = viewStateHelper;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesDeploymentsAction(FetchKubernetesDeploymentsAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<DeploymentViewModel>? items = await _viewStateHelper.GetDeployments(action.Tab.ContextState, action.SelectedNamespaces, action.CancellationToken);
        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Deployment, items);
        dispatcher.Dispatch(new FetchKubernetesDeploymentsActionResult(action.Tab, items ?? []));
    }
}