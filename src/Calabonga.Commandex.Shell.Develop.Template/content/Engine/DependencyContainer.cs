using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Extensions;
using Calabonga.Commandex.Engine.Processors;
using Calabonga.Commandex.Engine.Processors.Base;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Engine.ViewModelLocator;
using Calabonga.Commandex.Shell.Develop.Sample;
using Calabonga.Commandex.Shell.Develop.ViewModels;
using Calabonga.Commandex.Shell.Develop.Views;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calabonga.Commandex.Shell.Develop.Engine;

/// <summary>
/// Dependency registration root
/// </summary>
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

        // default engine
        services.AddSingleton<DefaultDialogView>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowsViewModel>();
        services.AddSingleton<SettingsViewModel>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAppSettings>(_ => App.Current.Settings);
        services.AddSingleton<ISettingsReaderConfiguration, DefaultSettingsReaderConfiguration>();

        // result processor 
        services.AddSingleton<IResultProcessor, AdvancedResultProcessor>();
        services.AddSingleton<IProcessor, Processor>();

        // dialogs and wizard
        services.AddDialogComponent();
        services.AddWizardComponent();

        // --------------------------------------------------
        // 1. Attach command definition from your project where Commandex.Command implemented.
        // 2. Then uncomment line below and add your command type.
        // services.AddDefinitions(typeof(YourAppDefinition)); // <-- uncomment this line and register your command here
        services.AddDefinitions(typeof(SampleCommandDefinition)); // <-- and comment this line
        // --------------------------------------------------

        var buildServiceProvider = services.BuildServiceProvider();
        ViewModelLocationProvider.SetDefaultViewModelFactory(type => buildServiceProvider.GetRequiredService(type));
        return buildServiceProvider;
    }
}
