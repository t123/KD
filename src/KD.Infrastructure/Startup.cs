using KD.Infrastructure.k8s;
using KD.Infrastructure.k8s.ViewModels;
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
        serviceCollection.AddTransient<IViewStateHelper, ViewStateHelper>();
#if DEBUG
        serviceCollection.AddTransient<IKubernetesDataLoader, DummyKubernetesDataLoader>();
#else
        serviceCollection.AddTransient<IKubernetesDataLoader, KubernetesDataLoader>();
#endif
    }
}
