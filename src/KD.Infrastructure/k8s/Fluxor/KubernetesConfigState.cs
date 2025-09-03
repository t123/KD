using Fluxor;

namespace KD.Infrastructure.k8s.Fluxor;

[FeatureState]
public class KubernetesConfigState
{
    public bool IsLoading { get; }
    public Config? CurrentConfig { get; }
    public Config[] Configs { get; }

    private KubernetesConfigState()
    {
        IsLoading = true;
        CurrentConfig = null;
        Configs = [];
    }

    public KubernetesConfigState(bool isLoading, Config? currentConfig, Config[]? configs)
    {
        IsLoading = isLoading;
        CurrentConfig = currentConfig;
        Configs = configs ?? [];
    }
}

public record FetchKubernetesConfigAction();

public record FetchKubernetesConfigResultAction(Config? CurrentConfig = null, Config[]? Configs = null);

public static partial class Reducers
{
    [ReducerMethod]
    public static KubernetesConfigState ReduceFetchKubernetesConfigs(KubernetesConfigState state, FetchKubernetesConfigAction action)
        => new KubernetesConfigState(true, null, null);

    [ReducerMethod]
    public static KubernetesConfigState ReduceFetchKubernetesConfigs(KubernetesConfigState state, FetchKubernetesConfigResultAction action)
        => new KubernetesConfigState(false, action.CurrentConfig, action.Configs);
}

public class KubernetesConfigEffects
{
    private readonly ConfigsManager _configsManager;

    public KubernetesConfigEffects(ConfigsManager configsManager)
    {
        _configsManager = configsManager;
    }

    [EffectMethod]
    public async Task HandleFetchKubernetesConfigAction(FetchKubernetesConfigAction action, IDispatcher dispatcher)
    {
        var configs = _configsManager.GetConfigs();
        var config = _configsManager.CurrentConfig;

        dispatcher.Dispatch(new FetchKubernetesConfigResultAction(config, configs));
    }
}