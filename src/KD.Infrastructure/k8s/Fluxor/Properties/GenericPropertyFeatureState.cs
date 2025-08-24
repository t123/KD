using KD.Infrastructure.k8s.ViewModels.Properties;

namespace KD.Infrastructure.k8s.Fluxor.Properties;

public record GenericPropertyFeatureState<T> where T : IPropertyViewModel
{
    public bool IsLoading { get; set; }
    public GenericProperty<T>? Property { get; set; }
}

public record GenericProperty<T> where T : IPropertyViewModel
{
    public T? Item { get; set; } = default(T);
    public DateTime? LastUpdate { get; set; }
}

public static partial class Reducers
{
    private static GenericPropertyFeatureState<T> FetchStateBegin<T>(GenericPropertyFeatureState<T> state, FetchKubernetesGenericPropertyAction<T> action) where T : IPropertyViewModel
    {
        var newState = state with { IsLoading = true };
        return newState!;
    }

    private static GenericPropertyFeatureState<T> FetchStateResult<T>(GenericPropertyFeatureState<T> state, FetchKubernetesGenericPropertyActionResult<T> action) where T : IPropertyViewModel
    {
        var newState = state with
        {
            IsLoading = false
        };

        if (newState.Property == null)
        {
            newState.Property = new()
            {
                Item = action.Item,
                LastUpdate = DateTime.Now
            };
        }
        else
        {
            newState.Property.LastUpdate = DateTime.Now;
            newState.Property.Item = action.Item;
        }

        return newState;
    }
}

public record FetchKubernetesGenericPropertyAction<T>(TabModel Tab, CancellationToken CancellationToken = default) where T : IPropertyViewModel;
public record FetchKubernetesGenericPropertyActionResult<T>(TabModel Tab, T Item, CancellationToken CancellationToken = default) where T : IPropertyViewModel;
