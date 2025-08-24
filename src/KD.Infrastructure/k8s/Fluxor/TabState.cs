using Fluxor;

namespace KD.Infrastructure.k8s.Fluxor;

[FeatureState]
public record TabState
{
    public bool IsLoading { get; set; }
    public TabModel[] Tabs { get; set; } = [];
    public TabModel? CurrentTab { get; set; }

    public TabState()
    {
        IsLoading = false;
        Tabs = [];
        CurrentTab = null;
    }
}

public record TabModel(Config ConfigState, Context ContextState, string ObjectViewType)
{
    public string Name => $"{ContextState.Name} - {ObjectViewType}";
}

public record AddTabAction(Config ConfigState, Context ContextState, string ObjectViewType);
public record AddTabActionResult(Config ConfigState, Context ContextState, string ObjectViewType);
public record CloseTabAction(TabModel Tab);

public static partial class Reducers
{
    [ReducerMethod]
    public static TabState ReduceAddTabAction(TabState state, AddTabAction action)
    {
        var tab = new TabModel(action.ConfigState, action.ContextState, action.ObjectViewType);

        var exists = state.Tabs.SingleOrDefault(x => x.Name == tab.Name);

        if (exists == null)
        {
            return state with { Tabs = [.. state.Tabs, tab], CurrentTab = tab };
        }

        return state with { CurrentTab = exists };
    }

    [ReducerMethod]
    public static TabState ReduceHandleAddTabActionResult(TabState state, CloseTabAction action)
    {
        var exists = state.Tabs.SingleOrDefault(x => x.Name == action.Tab.Name);

        if (exists == null)
        {
            return state;
        }

        var newTabs = state.Tabs.Where(x => x.Name != exists.Name).ToArray();
        return state with { Tabs = newTabs };
    }
}