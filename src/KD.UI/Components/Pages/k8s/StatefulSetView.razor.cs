using Fluxor;
using KD.Infrastructure.k8s.Fluxor.Objects;
using Microsoft.AspNetCore.Components;

namespace KD.UI.Components.Pages.k8s;

public partial class StatefulSetView : BaseView
{
    [Inject]
    public IState<StatefulSetViewState> State { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Dispatcher.Dispatch(new FetchKubernetesStatefulSetAction(Tab, NamespacesState.Value.SelectedNamespaces, _cancellationTokenSource.Token));
    }
}