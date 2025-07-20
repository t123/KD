namespace KD.Infrastructure;

public static class Colour
{
    public const string Info = "Info";
    public const string Success = "Success";
    public const string Error = "Error";
    public const string Warning = "Warning";
    public const string Primary = "Primary";
    public const string Secondary = "Secondary";
    public const string Tertiary = "Tertiary";

    public static string[] All = [Info, Success, Error, Warning, Primary, Secondary, Tertiary];

    public static MudBlazor.Color ToMudBlazor(string? s)
    {
        switch (s)
        {
            case Info: return MudBlazor.Color.Info;
            case Success: return MudBlazor.Color.Success;
            case Error: return MudBlazor.Color.Error;
            case Warning: return MudBlazor.Color.Warning;
            case Primary: return MudBlazor.Color.Primary;
            case Secondary: return MudBlazor.Color.Secondary;
            case Tertiary: return MudBlazor.Color.Tertiary;
            default: return MudBlazor.Color.Default;
        }
    }
}

public class ConfigsManager
{
    public Config CurrentConfig { get; set; } = new Config("local");
    public Config[] GetConfigs() => new[] { CurrentConfig };
}

public class ContextsManager(IKubernetesClientManager clientManager)
{
    private readonly IKubernetesClientManager _clientManager = clientManager;
    public Context? CurrentContext { get; private set; }
    public Context[] GetContexts()
    {
        int i = 0;
        var contexts = _clientManager.Config.Contexts.Select(x => new Context(x.Name, Colour.All[i++])).ToArray();
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

public record Config(string Name);
public record Context(string Name, string Colour);