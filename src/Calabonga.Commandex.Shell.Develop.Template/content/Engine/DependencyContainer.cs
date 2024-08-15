using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Engine.Wizards;
using Calabonga.Commandex.Shell.Develop.Core;
using Calabonga.Commandex.Shell.Develop.Views;
using Calabonga.Commandex.Shell.Views.Dialogs;
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

        // dialogs and wizard
        services.AddTransient<IWizardView, Wizard>();
        services.AddTransient<IDialogService, DialogService>();
        services.AddTransient(typeof(IWizardManager<>), typeof(WizardManager<>));

        // --------------------------------------------------
        // 1. Attach command definition from your project where Commandex.Command implemented.
        // 2. Then uncomment line below and add your command type.
        // services.AddDefinitions(typeof(WelcomeAppDefinition)); // <-- here should be your Command
        // --------------------------------------------------

        return services.BuildServiceProvider();
    }
}