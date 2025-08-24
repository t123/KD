using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record RolePropertyViewState : GenericPropertyFeatureState<RolePropertyViewModel>;
public record FetchKubernetesRolePropertyAction(TabModel Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<RolePropertyViewModel>(Tab, CancellationToken);
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
    private readonly IIndexManager _indexManager;

    public RolePropertyViewStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesRolePropertyAction action, IDispatcher dispatcher)
    {
        var properties = new RolePropertyViewModel()
        {
            Created = DateTime.Now,
            Name = "test",
            Tab = action.Tab,
            Uid = Guid.NewGuid().ToString()
        };

        dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
        dispatcher.Dispatch(new FetchKubernetesRolePropertyActionResult(action.Tab, properties, action.CancellationToken));
    }
}