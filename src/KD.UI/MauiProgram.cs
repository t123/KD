using Examine;
using Fluxor;
using KD.Infrastructure;
using KD.Infrastructure.k8s.Fluxor;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using MudExtensions.Services;
using Serilog;

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

        builder.Services.AddSerilog(
            new LoggerConfiguration()
                .WriteTo.File(
                    Path.Combine(FileSystem.Current.AppDataDirectory, "log.txt"),
                    rollingInterval: RollingInterval.Day,
                    fileSizeLimitBytes: 10 * 1024 * 1024,
                    retainedFileCountLimit: 30
                )
                .CreateLogger()
        );

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        builder.Services.AddMudServices();
        builder.Services.AddMudExtensions();

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
