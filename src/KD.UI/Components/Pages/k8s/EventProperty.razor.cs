using Fluxor;
using KD.Infrastructure.k8s.Fluxor.Properties;
using Microsoft.AspNetCore.Components;

namespace KD.UI.Components.Pages.k8s;

public partial class EventProperty : BaseProperty
{

    [Inject]
    public IState<EventsPropertyViewState> State { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }
}