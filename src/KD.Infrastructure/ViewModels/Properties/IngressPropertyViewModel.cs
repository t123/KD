using k8s;
using k8s.Models;

namespace KD.Infrastructure.ViewModels.Properties;

public class IngressPropertyViewModel : BasePropertyViewModel
{
    IngressPropertyViewModel()
    {
    }

    public override string PropertyViewType => ObjectType.Ingress;

    public async static Task<IngressPropertyViewModel> Create(IPropertyViewModelContext context)
    {
        var ingress = await context.Client.ReadNamespacedIngressAsync(context.ViewModel.Name, namespaceParameter: context.ViewModel.Namespace);

        return new IngressPropertyViewModel()
        {
            Created = ingress.Metadata.CreationTimestamp,
            Name = context.ViewModel.Name,
            Tab = context.Tab,
            Uid = ingress.Uid()
        };
    }
}

