using Examine;
using Fluxor;
using static KD.Infrastructure.Fluxor.SearchState;

namespace KD.Infrastructure.Fluxor;

[FeatureState]
public record SearchState
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
    public SearchResult[]? Results { get; set; }
}

public record OpenSearchAction(CancellationToken CancellationToken = default);
public record CloseSearchAction(CancellationToken CancellationToken = default);
public record SearchAction(string Text, CancellationToken CancellationToken = default);
public record SearchActionResult(SearchStatus Status, IEnumerable<SearchResult>? Results, CancellationToken CancellationToken = default);

public static partial class Reducers
{
    [ReducerMethod]
    public static SearchState ReduceOpenSearchAction(SearchState state, OpenSearchAction action)
        => state with { IsOpen = true, Results = null, Status = SearchStatus.Initial };

    [ReducerMethod]
    public static SearchState ReduceCloseSearchAction(SearchState state, CloseSearchAction action)
        => state with { IsOpen = false };

    [ReducerMethod]
    public static SearchState ReduceSearchAction(SearchState state, SearchAction action)
        => state with { Status = SearchStatus.Searching };

    [ReducerMethod]
    public static SearchState ReduceSearchActionResult(SearchState state, SearchActionResult action)
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