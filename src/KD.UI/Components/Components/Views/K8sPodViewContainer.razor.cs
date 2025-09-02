using KD.Infrastructure.k8s.ViewModels.Objects;
using KD.UI.Components.Pages.k8s;
using Microsoft.AspNetCore.Components;

namespace KD.UI.Components.Components.Views;

public partial class K8sPodViewContainer : BaseView
{
    [Parameter]
    public required ContainerViewModel[] Containers { get; set; }

    [Parameter]
    public required bool InitContainers { get; set; }

    [Parameter]
    public required EventCallback<string> OnShellClicked { get; set; }

    private async Task OnTerminalClick(string containerName)
    {
        await OnShellClicked.InvokeAsync(containerName);
    }

    private async Task OnLogClick(string containerName)
    {
        await OnShellClicked.InvokeAsync(containerName);
    }
}