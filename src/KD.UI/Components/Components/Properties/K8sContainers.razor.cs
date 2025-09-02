using k8s.Models;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace KD.UI.Components.Components.Properties;
public partial class K8sContainers
{
    [Parameter]
    public required V1ContainerStatus? StatusContainer { get; set; }
    [Parameter]
    public required V1Container Container { get; set; }
    [Parameter]
    public required V1Pod Pod { get; set; }

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
            UseShellExecute = true
        });
    }
}