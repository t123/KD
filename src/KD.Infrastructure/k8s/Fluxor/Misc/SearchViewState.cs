using Fluxor;
using KD.Infrastructure.k8s.ViewModels.Search;
using static KD.Infrastructure.k8s.Fluxor.Misc.SearchViewState;

namespace KD.Infrastructure.k8s.Fluxor.Misc;

[FeatureState]
public record SearchViewState
{
    public enum SearchStatus
    {
        Initial,
        Searching,
        Results,
        Failure
    };

    public bool IsOpen { get; set; } = false;
    public SearchStatus Status { get; set; } = SearchStatus.Initial;
    public KDSearchResult[]? Results { get; set; }
}

public record OpenSearchAction(CancellationToken CancellationToken = default);
public record CloseSearchAction(CancellationToken CancellationToken = default);
public record SearchAction(string Text, CancellationToken CancellationToken = default);
public record SearchActionResult(SearchStatus Status, IEnumerable<KDSearchResult>? Results, CancellationToken CancellationToken = default);

public static partial class Reducers
{
    [ReducerMethod]
    public static SearchViewState ReduceOpenSearchAction(SearchViewState state, OpenSearchAction action)
        => state with { IsOpen = true, Results = null, Status = SearchStatus.Initial };

    [ReducerMethod]
    public static SearchViewState ReduceCloseSearchAction(SearchViewState state, CloseSearchAction action)
        => state with { IsOpen = false };

    [ReducerMethod]
    public static SearchViewState ReduceSearchAction(SearchViewState state, SearchAction action)
        => state with { Status = SearchStatus.Searching };

    [ReducerMethod]
    public static SearchViewState ReduceSearchActionResult(SearchViewState state, SearchActionResult action)
    {
        var newState = state with { Status = action.Status, Results = action.Results?.ToArray() ?? [] };
        return newState;
    }
}

public class SearchStateEffects
{
    private readonly IIndexManager _indexManager;

    public SearchStateEffects(IIndexManager indexManager)
    {
        _indexManager = indexManager;
    }

    [EffectMethod]
    public async Task HandleOpenSearchAction(OpenSearchAction action, IDispatcher dispatcher)
    {
        object a = new CloseSearchAction();
        dispatcher.Dispatch(new OpenOverlayAction(a, action.CancellationToken));
    }

    [EffectMethod]
    public async Task HandleCloseSearchAction(CloseSearchAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new CloseOverlayAction(action.CancellationToken));
    }

    [EffectMethod]
    public async Task HandleSearchAction(SearchAction action, IDispatcher dispatcher)
    {
        if (string.IsNullOrWhiteSpace(action.Text))
        {
            dispatcher.Dispatch(new SearchActionResult(SearchStatus.Initial, null));
            return;
        }

        try
        {
            var results = await _indexManager.Search(action.Text);
            dispatcher.Dispatch(new SearchActionResult(SearchStatus.Results, results));
        }
        catch
        {
            dispatcher.Dispatch(new SearchActionResult(SearchStatus.Failure, null));
            return;
        }
    }
}