using Fluxor;

namespace KD.Infrastructure.k8s.Fluxor.Misc;

[FeatureState]
public record OverlayViewState
{
    public bool IsOpen { get; set; } = false;
    public object? CloseAction { get; set; }
}

public record OpenOverlayAction(object? CloseAction, CancellationToken CancellationToken = default);
public record CloseOverlayAction(CancellationToken CancellationToken = default);

public static partial class Reducers
{
    [ReducerMethod]
    public static OverlayViewState ReduceOpenOverlayAction(OverlayViewState state, OpenOverlayAction action)
        => state with { IsOpen = true, CloseAction = action.CloseAction };

    [ReducerMethod]
    public static OverlayViewState ReduceCloseOverlayAction(OverlayViewState state, CloseOverlayAction action)
        => state with { IsOpen = false, CloseAction = null };
}