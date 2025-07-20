using k8s;
using k8s.Models;

namespace KD.Infrastructure.ViewModels.Properties;

public class PodPropertyViewModel : BasePropertyViewModel
{
    PodPropertyViewModel()
    {
    }

    public override string PropertyViewType => ObjectType.Pod;

    public async static Task<PodPropertyViewModel> Create(IPropertyViewModelContext context)
    {
        var pod = await context.Client.ReadNamespacedPodAsync(context.ViewModel.Name, context.ViewModel.Namespace);

        return new PodPropertyViewModel()
        {
            Created = pod.Metadata.CreationTimestamp,
            Name = context.ViewModel.Name,
            Tab = context.Tab,
            Uid = pod.Uid()
        };
    }
}
