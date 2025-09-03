using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record DeploymentPropertyViewState : GenericPropertyFeatureState<DeploymentPropertyViewModel>;
public record FetchKubernetesDeploymentPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<DeploymentPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesDeploymentPropertyActionResult(TabModel Tab, DeploymentPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<DeploymentPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static DeploymentPropertyViewState ReduceFetchKubernetesDeploymentPropertyAction(DeploymentPropertyViewState state, FetchKubernetesDeploymentPropertyAction action)
        => (FetchStateBegin(state, action) as DeploymentPropertyViewState)!;

    [ReducerMethod]
    public static DeploymentPropertyViewState ReduceFetchKubernetesDeploymentPropertyActionResult(DeploymentPropertyViewState state, FetchKubernetesDeploymentPropertyActionResult action)
        => (FetchStateResult(state, action) as DeploymentPropertyViewState)!;
}

internal class DeploymentPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public DeploymentPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesDeploymentPropertyAction action, IDispatcher dispatcher)
    {
        var deployment = await _viewStateHelper.GetDeployment(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (deployment != null)
        {
            var properties = new DeploymentPropertyViewModel()
            {
                Created = deployment.Metadata.CreationTimestamp,
                Name = deployment.Metadata.Name,
                Tab = action.Tab,
                Uid = deployment.Metadata.Uid,
                Deployment = deployment
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesDeploymentPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}