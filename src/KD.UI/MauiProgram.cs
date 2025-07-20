using Examine;
using Fluxor;
using KD.Infrastructure;
using KD.Infrastructure.Fluxor;
using KD.Infrastructure.ViewModels.Objects;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;

namespace KD.UI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddMudServices();
        builder.Services.AddFluxor(options =>
        {
            options.ScanAssemblies(typeof(KubernetesConfigState).Assembly);
        });
        builder.Services.AddExamine();
        builder.Services.AddExamineLuceneIndex(IndexManager.IndexName);
        builder.Services.AddCustomServices();

        return builder.Build();
    }
}
