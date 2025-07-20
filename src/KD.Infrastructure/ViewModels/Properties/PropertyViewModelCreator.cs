namespace KD.Infrastructure.ViewModels.Properties;

public static class PropertyViewModelCreator
{
    public static async Task<IPropertyViewModel> Create(IPropertyViewModelContext context)
    {
        switch(context.PropertyViewType)
        {
            case ObjectType.Pod: return await PodPropertyViewModel.Create(context);
            case ObjectType.Deployment: return await DeploymentPropertyViewModel.Create(context);
            case ObjectType.Namespace: return await NamespacePropertyViewModel.Create(context);
            case ObjectType.Ingress: return await IngressPropertyViewModel.Create(context);
            case ObjectType.Service: return await ServicePropertyViewModel.Create(context);
            case ObjectType.Endpoint: return await EndpointPropertyViewModel.Create(context);

            default: throw new Exception($"Unknown property view type: {context.PropertyViewType}");
        }
    }
}