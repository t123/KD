using BlazorMonaco.Editor;
using Fluxor;
using Fluxor.Blazor.Web.Components;
using KD.Infrastructure.k8s.Fluxor.Misc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using IDispatcher = Fluxor.IDispatcher;

namespace KD.UI.Components.Components;

public partial class YamlEditor : FluxorComponent
{
    private static bool OnDidInitSuccess = false;

    private StandaloneCodeEditor _standaloneCodeEditor;

    [Inject]
    private IState<EditorViewState> EditorState { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    protected override void OnInitialized()
    {
        SubscribeToAction<OpenEditorActionResult>(async (action) =>
        {
            if (_standaloneCodeEditor == null)
            {
                return;
            }

            var yaml = EditorState.Value.Yaml;

            int i = 0;

            System.Diagnostics.Debug.WriteLine("SubscribeToAction: Start");

            while (!OnDidInitSuccess && i < 5)
            {
                System.Diagnostics.Debug.WriteLine("SubscribeToAction: Delaying");

                await Task.Delay(250);
                i++;
            }

            if (!OnDidInitSuccess)
            {
                return;
            }

            System.Diagnostics.Debug.WriteLine("SubscribeToAction: End");

            await _standaloneCodeEditor.SetValue(yaml);
        });

        base.OnInitialized();
    }

    private async Task OnDidInit()
    {
        if (_standaloneCodeEditor != null)
        {
            await BlazorMonaco.Editor.Global.SetTheme(jsRuntime, "vs-dark");
            OnDidInitSuccess = true;
            System.Diagnostics.Debug.WriteLine("OnDidInit: End");
        }
    }

    private StandaloneEditorConstructionOptions EditorConstructionOptions(StandaloneCodeEditor editor)
    {
        return new StandaloneEditorConstructionOptions
        {
            AutomaticLayout = true,
            Language = "yaml",
            Value = " "
        };
    }

    private async Task CloseEditor(MouseEventArgs e)
    {
        OnDidInitSuccess = false;
        Dispatcher.Dispatch(new CloseEditorAction());
    }
}