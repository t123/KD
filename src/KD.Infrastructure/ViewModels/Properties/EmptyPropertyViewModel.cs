namespace KD.Infrastructure.ViewModels.Properties;

public class EmptyPropertyViewModel : BasePropertyViewModel
{
    public override string PropertyViewType => ObjectType.Empty;

    EmptyPropertyViewModel()
    {
    }

    public static async Task<EmptyPropertyViewModel> Create(IPropertyViewModelContext context)
    {
        return new EmptyPropertyViewModel()
        {
            Created = null,
            Name = context.ViewModel.Name,
            Tab = context.Tab,
            Uid = context.ViewModel.Uid
        };
    }
}