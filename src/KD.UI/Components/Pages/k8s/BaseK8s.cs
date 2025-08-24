using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Color = MudBlazor.Color;

using IDispatcher = Fluxor.IDispatcher;

namespace KD.UI.Components.Pages.k8s;

public class BaseK8s : FluxorComponent
{
    protected CancellationTokenSource _cancellationTokenSource = new();
    
    [Inject]
    protected IJSRuntime JsRuntime { get; set; }
    
    [Inject]
    public IDispatcher Dispatcher { get; set; }

    protected async Task CopyToClipboard(string? text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        await JsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }

    protected async Task OpenInNewWindow(string url)
    {
        if (!url.StartsWith("https://"))
        {
            url = "https://" + url;
        }

        await JsRuntime.InvokeVoidAsync("open", url, "_blank");
    }

    protected Color GetTextIndicatorColor(Func<bool> pred) => pred.Invoke() == true ? Color.Success : Color.Warning;

    protected Color GetTextIndicatorColor(string? phase) =>
        phase switch
        {
            "Running" => Color.Success,
            "Succeeded" => Color.Success,
            "True" => Color.Success,
            "Active" => Color.Success,

            "Pending" => Color.Warning,
            "Waiting" => Color.Warning,

            "Unavailable" => Color.Error,
            "Terminated" => Color.Error,

            "Info" => Color.Info,

            _ => Color.Info
        };
}
