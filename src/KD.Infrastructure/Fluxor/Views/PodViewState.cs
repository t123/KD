using Fluxor;
using k8s;
using k8s.Models;
using KD.Infrastructure.ViewModels.Objects;
using static KD.Infrastructure.ViewModels.Objects.ContainerViewModel;

namespace KD.Infrastructure.Fluxor.Views;

[FeatureState]
public record PodViewState : GenericFeatureState<PodViewModel>;
public record FetchKubernetesPodAction(Tab Tab, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewAction<PodViewModel>(Tab, CancellationToken);
public record FetchKubernetesPodActionResult(Tab Tab, IEnumerable<PodViewModel> Items, CancellationToken CancellationToken = default) : FetchKubernetesGenericViewActionResult<PodViewModel>(Tab, Items, CancellationToken);

public static partial class Reducers
{
    [ReducerMethod]
    public static PodViewState ReduceFetchKubernetesPodAction(PodViewState state, FetchKubernetesPodAction action)
        => FetchStateBegin(state, action) as PodViewState;

    [ReducerMethod]
    public static PodViewState ReduceFetchKubernetesPodActionResult(PodViewState state, FetchKubernetesPodActionResult action)
        => FetchStateResult(state, action) as PodViewState;
}

public class PodViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;
    private readonly IIndexManager _indexManager;

    public PodViewStateEffects(IKubernetesClientManager clientManager, IIndexManager indexManager)
    {
        _clientManager = clientManager;
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesGenericViewAction(FetchKubernetesPodAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        IEnumerable<PodViewModel>? items = null;

        try
        {
            var client = _clientManager.GetClient(action.Tab.ContextState.Name);
            items = (await client.CoreV1.ListPodForAllNamespacesAsync(cancellationToken: action.CancellationToken))
                .Items
                .Select(x =>
                {
                    var readyAt = x.Status.Conditions.FirstOrDefault(x => x.Type == "Ready" && x.Status == "True")?.LastTransitionTime;
                    var pod = new PodViewModel(
                        x.Metadata.Uid,
                        x.Metadata.Name,
                        x.Metadata.NamespaceProperty,
                        x.Status.ContainerStatuses?.Select(y =>
                        {
                            ContainerState state = ContainerState.Unknown;
                            if (y.State.Terminated != null) state = ContainerState.Terminated;
                            else if (y.State.Waiting != null) state = ContainerState.Waiting;
                            else if (y.State.Running != null) state = ContainerState.Running;

                            var c = new ContainerViewModel(y.ContainerID, y.Name, y.Started ?? false, state, y.RestartCount);

                            return c;
                        }).ToArray() ?? Array.Empty<ContainerViewModel>(),
                        x.Status.InitContainerStatuses?.Select(y =>
                        {
                            ContainerState state = ContainerState.Unknown;
                            if (y.State.Terminated != null) state = ContainerState.Terminated;
                            else if (y.State.Waiting != null) state = ContainerState.Waiting;
                            else if (y.State.Running != null) state = ContainerState.Running;

                            var c = new ContainerViewModel(y.ContainerID, y.Name, y.Started ?? false, state, y.RestartCount);

                            return c;
                        }).ToArray() ?? Array.Empty<ContainerViewModel>(),
                        x.Status.Phase,
                        x.CreationTimestamp(),
                        readyAt,
                        x.Spec.NodeName
                    );

                    return pod;
                }
                )
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Namespace);
        }
        catch
        {
            items = null;
        }

        await _indexManager.IndexItems(action.Tab.ContextState.Name, ObjectType.Pod, items);
        var result = new FetchKubernetesPodActionResult(action.Tab, items ?? []);
        dispatcher.Dispatch(result);
    }
}