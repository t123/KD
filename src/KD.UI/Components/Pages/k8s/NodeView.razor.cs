using Fluxor;
using KD.Infrastructure.k8s.Fluxor;
using KD.Infrastructure.k8s.Fluxor.Objects;
using KD.Infrastructure.k8s.Fluxor.Properties;
using KD.Infrastructure.k8s.ViewModels.Objects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KD.UI.Components.Pages.k8s;

public partial class NodeView : BaseView
{
    private IObjectViewModel? _contextRow;

    [Inject]
    public IState<NodeViewState> State { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        SubscribeToAction<UpdateNamespacesSelectionAction>((action) => Fetch());

        Fetch();
    }

    protected void Fetch()
    {
        Dispatcher.Dispatch(new FetchKubernetesNodeAction(Tab, NamespacesState.Value.SelectedNamespaces, _cancellationTokenSource.Token));
    }

    private void OpenProperties(NodeViewModel viewModel)
    {
        Dispatcher.Dispatch(new OpenPropertiesAction(_cancellationTokenSource.Token));
        Dispatcher.Dispatch(new FetchKubernetesNodePropertyAction(Tab, viewModel.Name, viewModel.Namespace, _cancellationTokenSource.Token));
    }

    private async Task ContextMenuClick(DataGridRowClickEventArgs<NodeViewModel> args)
    {
        _contextRow = args.Item;

        if (_contextMenu != null)
        {
            await _contextMenu.OpenMenuAsync(args.MouseEventArgs);
        }
    }
}