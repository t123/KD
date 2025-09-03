using Fluxor;
using KD.Infrastructure.k8s;
using KD.Infrastructure.k8s.Fluxor;
using KD.Infrastructure.k8s.Fluxor.Misc;
using KD.Infrastructure.k8s.Fluxor.Objects;
using KD.Infrastructure.k8s.Fluxor.Properties;
using KD.Infrastructure.k8s.ViewModels.Objects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KD.UI.Components.Pages.k8s;

public partial class PodDisruptionBudgetView : BaseView
{
    private PodDisruptionBudgetViewModel _contextRow;

    [Inject]
    public IState<PodDisruptionBudgetViewState> State { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _refreshAction = () => Dispatcher.Dispatch(new FetchKubernetesPodDisruptionBudgetAction(Tab, NamespacesState.Value.SelectedNamespaces, _cancellationTokenSource.Token));

        SubscribeToAction<UpdateNamespacesSelectionAction>((action) => Fetch());

        Fetch();
    }

    private void OpenProperties(PodDisruptionBudgetViewModel viewModel)
    {
        Dispatcher.Dispatch(new OpenPropertiesAction(_cancellationTokenSource.Token));
        Dispatcher.Dispatch(new FetchKubernetesPodDisruptionBudgetPropertyAction(Tab, viewModel.Name, viewModel.Namespace, _cancellationTokenSource.Token));
    }

    protected async Task ContextMenuClick(DataGridRowClickEventArgs<PodDisruptionBudgetViewModel> args)
    {
        _contextRow = args.Item;
        if (_contextMenu != null)
        {
            await _contextMenu.OpenMenuAsync(args.MouseEventArgs);
        }
    }

    private async Task OpenEditor(TabModel tab, string name, string ns)
    {
        Dispatcher.Dispatch(new OpenEditorAction(tab, name, ns, ObjectType.PodDisruptionBudget));
    }
}