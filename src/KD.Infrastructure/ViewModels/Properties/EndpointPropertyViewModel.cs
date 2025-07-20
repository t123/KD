using k8s.Models;

namespace KD.Infrastructure.ViewModels.Properties;

public class EndpointPropertyViewModel : BasePropertyViewModel
{
    EndpointPropertyViewModel()
    {
    }

    public override string PropertyViewType => ObjectType.Endpoint;

    public static async Task<EndpointPropertyViewModel> Create(IPropertyViewModelContext context)
    {
        var vm = new EndpointPropertyViewModel()
        {
            Tab = context.Tab,
            Created = DateTime.Now,
            Name = context.ViewModel.Name,
            Uid = "uid"
        };

        return vm;
    }
}
