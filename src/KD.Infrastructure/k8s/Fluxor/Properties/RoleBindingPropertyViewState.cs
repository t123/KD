using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record RoleBindingPropertyViewState : GenericPropertyFeatureState<RoleBindingPropertyViewModel>;
public record FetchKubernetesRoleBindingPropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<RoleBindingPropertyViewModel>(Tab, CancellationToken);
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
    private readonly IIndexManager _indexManager;

    public RoleBindingPropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesRoleBindingPropertyAction action, IDispatcher dispatcher)
    {
        var properties = new RoleBindingPropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesRoleBindingPropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}