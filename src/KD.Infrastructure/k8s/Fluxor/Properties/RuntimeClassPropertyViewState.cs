using Fluxor;
using KD.Infrastructure.k8s.ViewModels;
using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

[FeatureState]
public record RuntimeClassPropertyViewState : GenericPropertyFeatureState<RuntimeClassPropertyViewModel>;
public record FetchKubernetesRuntimeClassPropertyAction(TabModel Tab, string Name, string Namespace, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyAction<RuntimeClassPropertyViewModel>(Tab, CancellationToken);
public record FetchKubernetesRuntimeClassPropertyActionResult(TabModel Tab, RuntimeClassPropertyViewModel Item, CancellationToken CancellationToken = default) : FetchKubernetesGenericPropertyActionResult<RuntimeClassPropertyViewModel>(Tab, Item, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static RuntimeClassPropertyViewState ReduceFetchKubernetesRuntimeClassPropertyAction(RuntimeClassPropertyViewState state, FetchKubernetesRuntimeClassPropertyAction action)
        => (FetchStateBegin(state, action) as RuntimeClassPropertyViewState)!;

    [ReducerMethod]
    public static RuntimeClassPropertyViewState ReduceFetchKubernetesRuntimeClassPropertyActionResult(RuntimeClassPropertyViewState state, FetchKubernetesRuntimeClassPropertyActionResult action)
        => (FetchStateResult(state, action) as RuntimeClassPropertyViewState)!;
}

internal class RuntimeClassPropertyViewStateEffects
{
    private readonly IViewStateHelper _viewStateHelper;

    public RuntimeClassPropertyViewStateEffects(IViewStateHelper viewStateHelper)
    {
        _viewStateHelper = viewStateHelper;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericPropertyAction(FetchKubernetesRuntimeClassPropertyAction action, IDispatcher dispatcher)
    {
        var runtimeClass = await _viewStateHelper.GetRuntimeClass(action.Tab.ContextState, action.Namespace, action.Name, action.CancellationToken);

        if (runtimeClass != null)
        {
            var properties = new RuntimeClassPropertyViewModel()
            {
                Created = runtimeClass.Metadata.CreationTimestamp,
                Name = runtimeClass.Metadata.Name,
                Tab = action.Tab,
                Uid = runtimeClass.Metadata.Uid,
                RuntimeClass = runtimeClass
            };

            dispatcher.Dispatch(new OpenPropertiesActionResult(properties, action.CancellationToken));
            dispatcher.Dispatch(new FetchKubernetesRuntimeClassPropertyActionResult(action.Tab, properties, action.CancellationToken));
        }
    }
}