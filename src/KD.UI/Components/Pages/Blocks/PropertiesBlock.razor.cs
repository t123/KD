using Fluxor;
using KD.Infrastructure.k8s.Fluxor.Properties;
using KD.UI.Code;
using KD.UI.Components.Pages.k8s;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace KD.UI.Components.Pages.Blocks;

public partial class PropertiesBlock : BaseView
{
    private DynamicComponent _dc;

    Type SelectedType
    {
        get
        {
            if (!PropertyViewState.Value.IsOpen || PropertyViewState.Value.ViewModel == null)
            {
                return typeof(EmptyProperty);
            }

            if (ObjectTypeViewMapping.Map.TryGetValue(PropertyViewState.Value.ViewModel.ObjectPropertyType, out var type) && type?.Property != null)
            {
                return type.Property;
            }

            return typeof(EmptyProperty);
        }
    }

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