using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Extensions;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Shell.Develop.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calabonga.Commandex.Shell.Develop.Engine;

internal static class DependencyContainer
{
    internal static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(options =>
        {
            options.AddSerilog(dispose: true);
            options.AddDebug();
        });

        services.AddSingleton<DefaultDialogView>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<ViewModels.MainWindowsViewModel>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAppSettings>(_ => App.Current.Settings);
        services.AddSingleton<ISettingsReaderConfiguration, DefaultSettingsReaderConfiguration>();

        // dialogs and wizard
        services.AddDialogComponent();
        services.AddWizardComponent();

        // --------------------------------------------------
        // 1. Attach command definition from your project where Commandex.Command implemented.
        // 2. Then uncomment line below and add your command type.
        // services.AddDefinitions(typeof(YourAppDefinition)); // <-- uncomment line and register your command here
        // --------------------------------------------------

        return services.BuildServiceProvider();
    }
}
