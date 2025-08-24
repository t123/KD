using Fluxor;
using KD.Infrastructure.k8s.Fluxor;
using KD.Infrastructure.k8s.Fluxor.Objects;
using KD.Infrastructure.k8s.ViewModels.Objects;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KD.UI.Components.Pages.k8s;


public partial class PersistentVolumeClaimView : BaseView
{
    private IObjectViewModel? _contextRow;

    [Inject]
    public IState<PersistentVolumeClaimViewState> State { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        SubscribeToAction<UpdateNamespacesSelectionAction>((action) => Fetch());

        Fetch();
    }

    protected void Fetch()
    {
        Dispatcher.Dispatch(new FetchKubernetesPersistentVolumeClaimAction(Tab, NamespacesState.Value.SelectedNamespaces, _cancellationTokenSource.Token));
    }

    private async Task ContextMenuClick(DataGridRowClickEventArgs<PersistentVolumeClaimViewModel> args)
    {
        _contextRow = args.Item;

        if (_contextMenu != null)
        {
            await _contextMenu.OpenMenuAsync(args.MouseEventArgs);
        }
    }
}