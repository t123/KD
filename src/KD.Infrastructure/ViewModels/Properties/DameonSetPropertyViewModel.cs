using k8s;
using k8s.Models;

namespace KD.Infrastructure.ViewModels.Properties;

public class DameonSetPropertyViewModel : BasePropertyViewModel
{
    DameonSetPropertyViewModel()
    {
    }

    public override string PropertyViewType => ObjectType.DaemonSet;

    public async static Task<DameonSetPropertyViewModel> Create(IPropertyViewModelContext context)
    {
        var deployment = await context.Client.ReadNamespacedDaemonSetAsync(context.ViewModel.Name, context.ViewModel.Namespace);

        return new DameonSetPropertyViewModel()
        {
            Created = deployment.Metadata.CreationTimestamp,
            Name = context.ViewModel.Name,
            Tab = context.Tab,
            Uid = deployment.Uid()
        };
    }
}

