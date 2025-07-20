using k8s;
using KD.Infrastructure.Fluxor;
using KD.Infrastructure.ViewModels.Objects;

namespace KD.Infrastructure.ViewModels.Properties;

public interface IPropertyViewModel
{
    string PropertyViewType { get; }
    Tab Tab { get; }
    string Uid { get; }
    string Name { get; }
    DateTime? Created { get; }
}

public abstract class BasePropertyViewModel : IPropertyViewModel
{
    public virtual string PropertyViewType => throw new NotImplementedException();
    public required Tab Tab { get; init; }
    public required string Uid { get; init; }
    public required string Name { get; init; }
    public required DateTime? Created { get; init; }
}

public interface IPropertyViewModelContext
{
    string PropertyViewType { get; }
    Kubernetes Client { get; }
    Tab Tab { get; }
    IObjectViewModel ViewModel { get; }
}

public record PropertyViewModelContext(string PropertyViewType, Kubernetes Client, Tab Tab, IObjectViewModel ViewModel) : IPropertyViewModelContext;

