using KD.Infrastructure.k8s.ViewModels.Objects;

namespace KD.Infrastructure.k8s.Fluxor.Objects;

public record GenericFeatureState<T> where T : IObjectViewModel
{
    public bool IsLoading { get; set; }
    public Dictionary<TabModel, GenericView<T>> TabMapping = new();
}

public record GenericView<T> where T : IObjectViewModel
{
    public IEnumerable<T> Items { get; set; } = [];
    public DateTime? LastUpdate { get; set; }
}

public static partial class Reducers
{
    private static GenericFeatureState<T> FetchStateBegin<T>(GenericFeatureState<T> state, FetchKubernetesGenericViewAction<T> action) where T : IObjectViewModel
    {
        var newState = state with { IsLoading = true };
        return newState!;
    }

    private static GenericFeatureState<T> FetchStateResult<T>(GenericFeatureState<T> state, FetchKubernetesGenericViewActionResult<T> action) where T : IObjectViewModel
    {
        var newState = state with { IsLoading = false };

        if (newState.TabMapping.ContainsKey(action.Tab))
        {
            newState.TabMapping[action.Tab].LastUpdate = DateTime.Now;
            newState.TabMapping[action.Tab].Items = action.Items;
        }
        else
        {
            newState.TabMapping[action.Tab] = new()
            {
                LastUpdate = DateTime.Now,
                Items = action.Items
            };
        }

        return newState;
    }
}

public record FetchKubernetesGenericViewAction<T>(TabModel Tab, CancellationToken CancellationToken = default) where T : IObjectViewModel;
public record FetchKubernetesGenericViewActionResult<T>(TabModel Tab, IEnumerable<T> Items, CancellationToken CancellationToken = default) where T : IObjectViewModel;
