using k8s;
using k8s.Models;

namespace KD.Infrastructure.ViewModels.Properties;

public class DeploymentPropertyViewModel : BasePropertyViewModel
{
    DeploymentPropertyViewModel()
    {
    }

    public override string PropertyViewType => ObjectType.Deployment;

    public async static Task<DeploymentPropertyViewModel> Create(IPropertyViewModelContext context)
    {
        var deployment = await context.Client.ReadNamespacedDeploymentAsync(context.ViewModel.Name, context.ViewModel.Namespace);

        return new DeploymentPropertyViewModel()
        {
            Created = deployment.Metadata.CreationTimestamp,
            Name = context.ViewModel.Name,
            Tab = context.Tab,
            Uid = deployment.Uid()
        };
    }
}

