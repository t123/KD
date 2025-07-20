using k8s;
using k8s.Models;

namespace KD.Infrastructure.ViewModels.Properties;

public class NamespacePropertyViewModel : BasePropertyViewModel
{
    NamespacePropertyViewModel()
    {
    }

    public override string PropertyViewType => ObjectType.Ingress;
    public required IReadOnlyDictionary<string, string> Annotations { get; init; }
    public required IReadOnlyDictionary<string, string> Labels { get; init; }
    public required string Status { get; init; }

    public static async Task<NamespacePropertyViewModel> Create(IPropertyViewModelContext context)
    {
        var ns = await context.Client.ReadNamespaceAsync(context.ViewModel.Name);

        var vm = new NamespacePropertyViewModel()
        {
            Tab = context.Tab,
            Created = ns.Metadata.CreationTimestamp,
            Name = context.ViewModel.Name,
            Status = ns.Status.Phase,
            Uid = ns.Uid(),
            Annotations = ns.Annotations().AsReadOnly(),
            Labels = ns.Annotations().AsReadOnly(),
        };

        return vm;
    }
}
