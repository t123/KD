using Fluxor;
using Fluxor.Blazor.Web.Components;
using KD.Infrastructure.k8s.Fluxor;
using KD.UI.Components.Pages.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.UI.Xaml.Controls;
using MudBlazor;
using IDispatcher = Fluxor.IDispatcher;

namespace KD.UI.Components.Pages.Blocks;

public partial class ObjectTabBlock : FluxorComponent
{
    private GenericView? _genericView;
    private MudDynamicTabs _mdt;

    [Inject]
    public IState<TabState> TabState { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    protected override void OnInitialized()
    {
        SubscribeToAction<AddTabAction>(TabAdded);
        base.OnInitialized();
    }

    private void TabAdded(AddTabAction action)
    {
        if (TabState.Value.Tabs.Length <= 1)
        {
            return;
        }

        var currentTab = TabState.Value.Tabs.SingleOrDefault(x => x.Name == TabState.Value.CurrentTab?.Name);

        if (currentTab != null)
        {
            _mdt.ActivatePanel(currentTab);
        }
    }

    private void CloseTab(MudTabPanel panel)
    {
        var tab = panel.ID as TabModel;

        if (tab == null)
        {
            return;
        }

        Dispatcher.Dispatch(new CloseTabAction(tab));
    }

    private void CloseAll()
    {
        Dispatcher.Dispatch(new CloseAllTabsAction());
    }
}