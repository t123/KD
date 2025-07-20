using Microsoft.Extensions.DependencyInjection;

namespace KD.Infrastructure;

public static class Startup
{
    public static void AddCustomServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IKubernetesClientManager, KubernetesClientManager>();
        serviceCollection.AddSingleton<ConfigsManager>();
        serviceCollection.AddSingleton<ContextsManager>();
        serviceCollection.AddTransient<IIndexManager, IndexManager>();
    }
}
