using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record CustomResourcesDefinitionViewState : GenericFeatureState<CustomResourcesDefinitionViewModel>;
public record FetchKubernetesCustomResourcesDefinitionAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<CustomResourcesDefinitionViewModel>(Tab, CancellationToken);
public record FetchKubernetesCustomResourcesDefinitionActionResult(Tab Tab, IEnumerable<CustomResourcesDefinitionViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<CustomResourcesDefinitionViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static CustomResourcesDefinitionViewState ReduceFetchKubernetesCustomResourcesDefinitionAction(CustomResourcesDefinitionViewState state, FetchKubernetesCustomResourcesDefinitionAction action)
        => FetchStateBegin(state, action) as CustomResourcesDefinitionViewState;

    [ReducerMethod]
    public static CustomResourcesDefinitionViewState ReduceFetchKubernetesCustomResourcesDefinitionActionResult(CustomResourcesDefinitionViewState state, FetchKubernetesCustomResourcesDefinitionActionResult action)
        => FetchStateResult(state, action) as CustomResourcesDefinitionViewState;
}

public class CustomResourcesDefinitionViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public CustomResourcesDefinitionViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesCustomResourcesDefinitionAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<CustomResourcesDefinitionViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.ListCustomResourceDefinitionAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x => new CustomResourcesDefinitionViewModel(
                    x.Uid(),
                    x.Name(),
                    x.Namespace(),
                    x.CreationTimestamp(),
                    x.Spec.Group,
                    x.Spec.Versions.Select(y => y.Name).ToArray(),
                    x.Spec.Scope
                ))
                .OrderBy(x => x.Name);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.CustomResourcesDefinition, items);
        dispatcher.Dispatch(new FetchKubernetesCustomResourcesDefinitionActionResult(action.Tab, items ?? []));
    }
}
