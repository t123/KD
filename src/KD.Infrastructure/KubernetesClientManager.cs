using k8s;
using k8s.KubeConfigModels;

namespace KD.Infrastructure;

public interface IKubernetesClientManager
{
    Kubernetes? GetClient(string context);
    K8SConfiguration Config { get; }
}

public class KubernetesClientManager : IKubernetesClientManager
{
    private Dictionary<string, Kubernetes> _clients;

    public Kubernetes? GetClient(string context)
    {
        if (context == null)
        {
            return null;
        }

        if (!_clients.ContainsKey(context))
        {
            var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(currentContext: context);
            var client = new Kubernetes(config);

            if (client != null)
            {
                _clients.Add(context, client);
            }
        }

        return _clients[context];
    }

    public K8SConfiguration Config { get; private set; }

    public KubernetesClientManager()
    {
        Config = KubernetesClientConfiguration.LoadKubeConfig();
        _clients = new Dictionary<string, Kubernetes>();
    }
}
