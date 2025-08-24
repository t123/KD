namespace KD.Infrastructure.k8s;

public record Config(string Name);

public class ConfigsManager
{
    public Config CurrentConfig { get; set; } = new Config("local");
    public Config[] GetConfigs() => [CurrentConfig];
}