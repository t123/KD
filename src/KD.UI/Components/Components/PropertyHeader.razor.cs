using Fluxor;
using KD.Infrastructure.k8s.Fluxor.Properties;
using KD.Infrastructure.k8s.ViewModels.Properties;
using KD.UI.Components.Pages.k8s;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KD.UI.Components.Components;

public partial class PropertyHeader<T> : BaseProperty where T : IPropertyViewModel
{
    [Parameter]
    public EventCallback OnRefresh { get; set; }

    [Parameter]
    public required T ViewModel { get; set; }

    [Inject]
    public IState<PropertyViewState> PropertyViewState { get; set; }

    private void ClosePropertyWindow(MouseEventArgs e)
    {
        Dispatcher.Dispatch(new ClosePropertiesAction(_cancellationTokenSource.Token));
    }

    private void ToggleWindow(MouseEventArgs e)
    {
        Dispatcher.Dispatch(new TogglePropertiesAction(PropertyViewState.Value.Width, _cancellationTokenSource.Token));
    }
}