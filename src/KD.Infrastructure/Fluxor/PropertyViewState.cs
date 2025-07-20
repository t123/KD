using Fluxor;
using KD.Infrastructure.ViewModels.Objects;
using KD.Infrastructure.ViewModels.Properties;

namespace KD.Infrastructure.Fluxor;

[FeatureState]
public record PropertyViewState
{
    public bool IsOpen { get; set; } = false;
    public string Width { get; set; } = "25%";
    public string Class { get; set; } = "width25";
    public bool IsLoading { get; set; }
    public DateTime? LastUpdate { get; set; }
    public IPropertyViewModel? ViewModel { get; set; }
}

public record OpenPropertiesAction(IObjectViewModel viewModel, string PropertyViewType, Tab Tab, CancellationToken CancellationToken = default);
public record OpenPropertiesActionResult(IPropertyViewModel? Property, CancellationToken CancellationToken = default);
public record ClosePropertiesAction(CancellationToken CancellationToken = default);
public record TogglePropertiesAction(string CurrentWidth, CancellationToken CancellationToken = default);
public record TogglePropertiesActionResult(string Width, string Cls, CancellationToken CancellationToken = default);

public static partial class Reducers
{
    [ReducerMethod]
    public static PropertyViewState ReduceFetchKubernetesProperties(PropertyViewState state, OpenPropertiesAction action)
        => state with { IsLoading = true, IsOpen = true };

    [ReducerMethod]
    public static PropertyViewState ReduceFetchKubernetesProperties(PropertyViewState state, ClosePropertiesAction action)
        => state with { IsOpen = false };

    [ReducerMethod]
    public static PropertyViewState ReduceOpenPropertiesActionResult(PropertyViewState state, OpenPropertiesActionResult action)
    {
        var newState = state with { IsLoading = false, LastUpdate = DateTime.Now, ViewModel = action.Property };
        return newState;
    }

    [ReducerMethod]
    public static PropertyViewState ReduceTogglePropertiesActionResult(PropertyViewState state, TogglePropertiesActionResult action)
        => state with { Width = action.Width, Class = action.Cls };
}

public class PropertyViewStateEffects
{
    private readonly IKubernetesClientManager _clientManager;

    public PropertyViewStateEffects(IKubernetesClientManager clientManager)
    {
        _clientManager = clientManager;
    }

    [EffectMethod]
    public async Task HandleOpenPropertiesAction(OpenPropertiesAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();
        var client = _clientManager.GetClient(action.Tab.ContextState.Name);

        if (client == null)
        {
            throw new Exception($"No null allowed {nameof(client)}");
        }

        var context = new PropertyViewModelContext(action.PropertyViewType, client, action.Tab, action.viewModel);
        IPropertyViewModel? property = await PropertyViewModelCreator.Create(context);
        dispatcher.Dispatch(new OpenPropertiesActionResult(property));
    }

    [EffectMethod]
    public async Task HandleTogglePropertiesAction(TogglePropertiesAction action, IDispatcher dispatcher)
    {
        action.CancellationToken.ThrowIfCancellationRequested();

        string newWidth = string.Empty;
        string newClass = string.Empty;
        switch (action.CurrentWidth)
        {
            case "25%":
                newWidth = "50%";
                newClass = "width50";
                break;

            case "50%":
                newWidth = "25%";
                newClass = "width25";
                break;

            default:
                newWidth = "25%";
                newClass = "width25";
                break;
        }

        dispatcher.Dispatch(new TogglePropertiesActionResult(newWidth, newClass));
    }
}