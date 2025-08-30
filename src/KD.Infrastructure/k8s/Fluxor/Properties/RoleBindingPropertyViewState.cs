using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record RoleBindingPropertyViewState : GenericPropertyFeatureState<RoleBindingPropertyViewModel>;
public record FetchKubernetesRoleBindingPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<RoleBindingPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesRoleBindingPropertyActionResult(TabModel Tab, RoleBindingPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<RoleBindingPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RoleBindingPropertyViewState ReduceFetchKubernetesRoleBindingPropertyAction(RoleBindingPropertyViewState state, FetchKubernetesRoleBindingPropertyAction action)
        => (FetchStateBegin(state, action) as RoleBindingPropertyViewState)!;

    [ReducerMethod]
    public static RoleBindingPropertyViewState ReduceFetchKubernetesRoleBindingPropertyActionResult(RoleBindingPropertyViewState state, FetchKubernetesRoleBindingPropertyActionResult action)
        => (FetchStateResult(state, action) as RoleBindingPropertyViewState)!;
}

internal class RoleBindingPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public RoleBindingPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesRoleBindingPropertyAction action, IDispatcher dispatcher)
    {
        var roleBinding = await _viewStateHelper.GetRoleBinding(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (roleBinding != null)
        {
            var properties = new RoleBindingPropertyViewModel()
            {
                Created = roleBinding.Metadata.CreationTimestamp,
                Name = roleBinding.Metadata.Name,
                Tab = action.Tab,
                Uid = roleBinding.Metadata.Uid,
                RoleBinding = roleBinding
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesRoleBindingPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}