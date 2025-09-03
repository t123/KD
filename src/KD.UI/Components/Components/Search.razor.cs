using Fluxor;
using Fluxor.Blazor.Web.Components;
using KD.Infrastructure.k8s.Fluxor.Misc;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using IDispatcher = Fluxor.IDispatcher;

namespace KD.UI.Components.Components;

public partial class Search : FluxorComponent
{
    [Inject]
    private IState<SearchViewState> SearchViewState { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    private string? TextValue { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    private void TextChanged(string s)
    {
        Dispatcher.Dispatch(new SearchAction(s));
    }

    private void CloseSearch()
    {
        Dispatcher.Dispatch(new CloseSearchAction());
    }

    private void KeyUp(KeyboardEventArgs args)
    {
        if (args.Code == "Escape")
        {
            Dispatcher.Dispatch(new CloseSearchAction());
        }
    }
}