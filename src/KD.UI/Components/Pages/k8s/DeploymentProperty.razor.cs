using Fluxor;
using KD.Infrastructure.k8s.Fluxor.Properties;
using Microsoft.AspNetCore.Components;

namespace KD.UI.Components.Pages.k8s;

public partial class DeploymentProperty : BaseProperty
{
    [Inject]
    public IState<DeploymentPropertyViewState> State { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}