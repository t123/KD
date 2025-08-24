using Fluxor;
using KD.Infrastructure.k8s.Fluxor.Objects;
using Microsoft.AspNetCore.Components;

namespace KD.UI.Components.Pages.k8s;

public partial class PodDisruptionBudgetView : BaseView
{
    [Inject]
    public IState<PodDisruptionBudgetViewState> State { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new FetchKubernetesPodDisruptionBudgetAction(Tab, NamespacesState.Value.SelectedNamespaces, _cancellationTokenSource.Token));
    }
}