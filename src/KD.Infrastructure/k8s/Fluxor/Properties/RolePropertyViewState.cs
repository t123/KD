using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record RolePropertyViewState : GenericPropertyFeatureState<RolePropertyViewModel>;
public record FetchKubernetesRolePropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<RolePropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesRolePropertyActionResult(TabModel Tab, RolePropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<RolePropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RolePropertyViewState ReduceFetchKubernetesRolePropertyAction(RolePropertyViewState state, FetchKubernetesRolePropertyAction action)
        => (FetchStateBegin(state, action) as RolePropertyViewState)!;

    [ReducerMethod]
    public static RolePropertyViewState ReduceFetchKubernetesRolePropertyActionResult(RolePropertyViewState state, FetchKubernetesRolePropertyActionResult action)
        => (FetchStateResult(state, action) as RolePropertyViewState)!;
}

internal class RolePropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public RolePropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesRolePropertyAction action, IDispatcher dispatcher)
    {
        var role = await _viewStateHelper.GetRole(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (role != null)
        {
            var properties = new RolePropertyViewModel()
            {
                Created = role.Metadata.CreationTimestamp,
                Name = role.Metadata.Name,
                Tab = action.Tab,
                Uid = role.Metadata.Uid,
                Role = role
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesRolePropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}