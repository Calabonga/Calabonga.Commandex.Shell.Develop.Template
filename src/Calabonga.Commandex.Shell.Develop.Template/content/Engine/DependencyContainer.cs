using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Extensions;
using Calabonga.Commandex.Engine.Processors;
using Calabonga.Commandex.Engine.Processors.Base;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Engine.ToastNotifications;
using Calabonga.Commandex.Engine.ViewModelLocator;
using Calabonga.Commandex.Engine.Zones;
using Calabonga.Commandex.Shell.Develop.ViewModels;
using Calabonga.Commandex.Shell.Develop.Views;
using Calabonga.Commandex.Shell.Develop.Zones;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calabonga.Commandex.Shell.Develop.Engine;
/// <summary>
/// Dependency registration root
/// </summary>
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
        services.AddSingleton<IZoneManager, ZoneManager>();
        services.AddSingleton<IMvvmObjectFactory, MvvmObjectFactory>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAppSettings>(_ => App.Current.Settings);
        services.AddSingleton<ISettingsReaderConfiguration, DefaultSettingsReaderConfiguration>();
        services.AddScoped<IPreviewView, PreviewView>();
        services.AddScoped<IPreviewViewModel, PreviewViewModel>();

        // toast notifications
        services.AddScoped<INotificationManager, NotificationManager>();

        // result processor 
        services.AddSingleton<IResultProcessor, AdvancedResultProcessor>();
        services.AddSingleton<IProcessor, Processor>();

        // dialogs and wizard
        services.AddDialogComponent();
        services.AddWizardComponent();

        // register all commands
        services.RegisterCommandsDefinitions();

        var buildServiceProvider = services.BuildServiceProvider();
        ViewModelLocationProvider.SetDefaultViewModelFactory(type => buildServiceProvider.GetRequiredService(type));
        return buildServiceProvider;
    }
}
