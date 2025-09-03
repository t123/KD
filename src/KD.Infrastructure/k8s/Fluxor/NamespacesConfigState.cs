using Fluxor;
using KD.Infrastructure.k8s.ViewModels;

namespace KD.Infrastructure.k8s.Fluxor;

[FeatureState]
public record NamespacesConfigState
{
    public bool IsLoading { get; set; }
    public string[] Namespaces { get; set; }
    public string? Selected { get; set; }
    public string[] SelectedNamespaces => Selected?.Split([','], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries) ?? [];

    public NamespacesConfigState()
    {
        IsLoading = true;
        Namespaces = [];
        Selected = string.Empty;
    }
}

public record FetchNamespacesConfigAction(Context? ContextState, CancellationToken CancellationToken = default);
public record FetchNamespacesConfigResultAction(string[] Namespaces, CancellationToken CancellationToken = default);
public record UpdateNamespacesSelectionAction(string Selected, CancellationToken CancellationToken = default);

public static partial class Reducers
{
    [ReducerMethod]
    public static NamespacesConfigState ReduceFetchNamespacesConfig(NamespacesConfigState state, FetchNamespacesConfigAction action)
        => new NamespacesConfigState();

    [ReducerMethod]
    public static NamespacesConfigState ReduceFetchNamespacesConfigResult(NamespacesConfigState state, FetchNamespacesConfigResultAction action)
        => state with { Namespaces = action.Namespaces, IsLoading = false };

    [ReducerMethod]
    public static NamespacesConfigState ReduceUpdateNamespacesSelectionAction(NamespacesConfigState state, UpdateNamespacesSelectionAction action)
    {
        if (string.IsNullOrWhiteSpace(state.Selected))
        {
            return state with { Selected = action.Selected };
        }

        if (action.Selected == null)
        {
            return state with { Selected = string.Empty };
        }

        if (state.SelectedNamespaces.Contains(action.Selected))
        {
            var s = state.SelectedNamespaces.Where(s => s != action.Selected);
            return state with { Selected = string.Join(',', s) };
        }
        else
        {
            return state with { Selected = state.Selected + "," + action.Selected };
        }
    }

    internal class NamespacesConfigEffects
    {
        private readonly IViewStateHelper _viewStateHelper;

        public NamespacesConfigEffects(IViewStateHelper viewStateHelper)
        {
            _viewStateHelper = viewStateHelper;
        }

        [EffectMethod]
        public async Task HandleFetchNamespacesConfigAction(FetchNamespacesConfigAction action, IDispatcher dispatcher)
        {
            var ns = action.ContextState == null
                ? []
                : await _viewStateHelper.GetNamespaces(action.ContextState, [], action.CancellationToken);

            var result = (ns ?? []).Select(x => x.Name).OrderBy(x => x, StringComparer.InvariantCultureIgnoreCase).ToArray();

            dispatcher.Dispatch(new FetchNamespacesConfigResultAction(result, action.CancellationToken));
        }
    }
}