using Fluxor;
using KD.Infrastructure.k8s.Fluxor.Properties;
using Microsoft.AspNetCore.Components;

namespace KD.UI.Components.Pages.k8s;

public partial class CronJobProperty : BaseProperty
{
    [Inject]
    public IState<CronJobPropertyViewState> State { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}