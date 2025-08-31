using Microsoft.Extensions.Logging;

namespace KD.UI;

public partial class App : Application
{
    public App(ILogger<App> logger)
    {
        AppDomain.CurrentDomain.FirstChanceException += (sender, error) =>
        {
            logger.LogError(error?.Exception, error?.Exception?.Message);
        };

        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new MainPage()) { Title = "KD" };
    }
}
