namespace KD.Infrastructure.k8s;

public class ContextsManager(IKubernetesClientManager clientManager)
{
    private readonly IKubernetesClientManager _clientManager = clientManager;
    public Context? CurrentContext { get; private set; }
    public Context[] GetContexts()
    {
        var contexts = _clientManager.Config.Contexts.Select(x => new Context(x.Name)).ToArray();
        return contexts;
    }

    public Context? ChangeContext(Context context)
    {
        CurrentContext = context;
        return CurrentContext;
    }

    public Context? FindContext(string name)
    {
        var context = GetContexts().SingleOrDefault(x => x.Name == name);
        return context;
    }
}

public record Context(string Name);
