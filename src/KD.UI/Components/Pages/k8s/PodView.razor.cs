using Fluxor;
using KD.Infrastructure.k8s;
using KD.Infrastructure.k8s.Fluxor;
using KD.Infrastructure.k8s.Fluxor.Misc;
using KD.Infrastructure.k8s.Fluxor.Objects;
using KD.Infrastructure.k8s.Fluxor.Properties;
using KD.Infrastructure.k8s.ViewModels.Objects;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics;
using IDispatcher = Fluxor.IDispatcher;

namespace KD.UI.Components.Pages.k8s;

public partial class PodView : BaseView
{
    private PodViewModel _contextRow;

    public bool? ShowInitContainers { get; set; } = false;
    public bool? ShowUnavailableContainers { get; set; } = false;

    [Inject]
    public IState<PodViewState> State { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _refreshAction = () => Dispatcher.Dispatch(new FetchKubernetesPodAction(Tab, NamespacesState.Value.SelectedNamespaces, _cancellationTokenSource.Token)); ;

        SubscribeToAction<UpdateNamespacesSelectionAction>((action) => Fetch());

        Fetch();
    }

    private void OpenProperties(PodViewModel viewModel)
    {
        Dispatcher.Dispatch(new OpenPropertiesAction(_cancellationTokenSource.Token));
        Dispatcher.Dispatch(new FetchKubernetesPodPropertyAction(Tab, viewModel.Name, viewModel.Namespace, _cancellationTokenSource.Token));
    }

    protected async Task ContextMenuClick(DataGridRowClickEventArgs<PodViewModel> args)
    {
        _contextRow = args.Item;
        if (_contextMenu != null)
        {
            await _contextMenu.OpenMenuAsync(args.MouseEventArgs);
        }
    }

    private async Task OpenEditor(TabModel tab, string name, string ns)
    {
        Dispatcher.Dispatch(new OpenEditorAction(tab, name, ns, ObjectType.Pod));
    }

    private void OpenShell(string ns, string containerName, string name)
    {
        var args = @$"exec -i -t -n {ns} {name} -c {containerName} -- sh -c ""clear; (bash || ash ||sh)""";

        var console = Process.Start(new ProcessStartInfo()
        {
            CreateNoWindow = false,
            FileName = "kubectl",
            Arguments = args,
            RedirectStandardInput = false,
            RedirectStandardOutput = false,
            RedirectStandardError = false,
            UseShellExecute = true,
        });
    }
}