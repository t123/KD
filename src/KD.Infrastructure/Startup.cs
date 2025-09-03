using KD.Infrastructure.k8s;
using KD.Infrastructure.k8s.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace KD.Infrastructure;

public static class Startup
{
    public static void AddCustomServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IBackgroundTaskQueue>(ctx => new BackgroundTaskQueue(100));
        serviceCollection.AddHostedService<KdBackgroundService>();
        serviceCollection.AddSingleton<IKubernetesClientManager, KubernetesClientManager>();
        serviceCollection.AddSingleton<ConfigsManager>();
        serviceCollection.AddSingleton<ContextsManager>();
        serviceCollection.AddTransient<IIndexManager, IndexManager>();
        serviceCollection.AddTransient<IViewStateHelper, ViewStateHelper>();
        serviceCollection.AddTransient<IKubernetesDataLoader, KubernetesDataLoader>();

//#if DEBUG
//        serviceCollection.AddTransient<IKubernetesDataLoader, DummyKubernetesDataLoader>();
//#else
//        serviceCollection.AddTransient<IKubernetesDataLoader, KubernetesDataLoader>();
//#endif
    }
}
