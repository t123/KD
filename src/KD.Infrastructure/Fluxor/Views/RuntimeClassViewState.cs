using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record RuntimeClassViewState : GenericFeatureState<RuntimeClassViewModel>;
public record FetchKubernetesRuntimeClassAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<RuntimeClassViewModel>(Tab, CancellationToken);
public record FetchKubernetesRuntimeClassActionResult(Tab Tab, IEnumerable<RuntimeClassViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<RuntimeClassViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RuntimeClassViewState ReduceFetchKubernetesRuntimeClassAction(RuntimeClassViewState state, FetchKubernetesRuntimeClassAction action)
        => FetchStateBegin(state, action) as RuntimeClassViewState;

    [ReducerMethod]
    public static RuntimeClassViewState ReduceFetchKubernetesRuntimeClassActionResult(RuntimeClassViewState state, FetchKubernetesRuntimeClassActionResult action)
        => FetchStateResult(state, action) as RuntimeClassViewState;
}

public class RuntimeClassViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public RuntimeClassViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesRuntimeClassAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<RuntimeClassViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListRuntimeClassAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new RuntimeClassViewModel(x.Uid(), x.Name(), x.Namespace(), x.CreationTimestamp()))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.RuntimeClass, items);
        dispatcher.Dispatch(new FetchKubernetesRuntimeClassActionResult(action.Tab, items ?? []));
    }
}
