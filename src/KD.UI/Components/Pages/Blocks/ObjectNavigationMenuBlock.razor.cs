using Fluxor;
using Fluxor.Blazor.Web.Components;
using KD.Infrastructure.k8s.Fluxor;
using KD.UI.Components.Pages.Common;
using Microsoft.AspNetCore.Components;
using IDispatcher = Fluxor.IDispatcher;

namespace KD.UI.Components.Pages.Blocks;

public partial class ObjectNavigationMenuBlock : FluxorComponent
{
    private GenericView? _genericView;

    [Inject]
    public IState<TabState> TabState { get; set; }

    [Inject]
    public IState<ObjectNavigationState> ObjectNavigationState { get; set; }

    [Inject]
    public IState<KubernetesContextState> ContextState { get; set; }

    [Inject]
    public IState<KubernetesConfigState> ConfigState { get; set; }

    [Inject]
    public IDispatcher Dispatcher { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Dispatcher.Dispatch(new FetchKubernetesConfigAction());
    }

    private void AddFavourite(string id)
    {
        Dispatcher.Dispatch(new ObjectNavigationAddFavouriteAction(id));
    }

    private void RemoveFavourite(string id)
    {
        Dispatcher.Dispatch(new ObjectNavigationRemoveFavouriteAction(id));
    }

    private void AddTab(string objectViewType)
    {
        if (ConfigState.Value.CurrentConfig == null) return;
        if (ContextState.Value.CurrentContext == null) return;

        Dispatcher.Dispatch(new AddTabAction(ConfigState.Value.CurrentConfig, ContextState.Value.CurrentContext, objectViewType));
    }
}