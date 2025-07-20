using k8s;
using k8s.Models;

namespace KD.Infrastructure.ViewModels.Properties;

public class ServicePropertyViewModel : BasePropertyViewModel
{
    ServicePropertyViewModel()
    {
    }

    public override string PropertyViewType => ObjectType.Service;

    public static async Task<ServicePropertyViewModel> Create(IPropertyViewModelContext context)
    {
        var vm = new ServicePropertyViewModel()
        {
            Tab = context.Tab,
            Created = DateTime.Now,
            Name = context.ViewModel.Name,
            Uid = "uid"
        };

        return vm;
    }
}
