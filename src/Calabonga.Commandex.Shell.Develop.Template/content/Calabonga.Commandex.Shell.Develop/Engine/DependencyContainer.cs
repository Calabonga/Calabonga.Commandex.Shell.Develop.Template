using Calabonga.Commandex.Engine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Calabonga.Commandex.Shell.Develop.Engine;

/// <summary>
/// Dependency container helper.
/// </summary>
internal static class DependencyContainer
{
    /// <summary>
    /// Executes some registrations for Commandex. You can use this register your CommandexCommand for testing
    /// </summary>
    /// <returns></returns>
    internal static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();


        services.AddLogging(options =>
        {
            options.AddSerilog(dispose: true);
            options.AddDebug();
        });

        services.AddSingleton(typeof(DefaultDialogResult<>));
        services.AddSingleton<DefaultDialogView>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowsViewModel>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IVersionService, VersionService>();

        // --------------------------------------------------
        // Attach definition from your library. Uncomment line below and add your command type.
        // services.AddDefinitions(typeof(YourCommandDefinition));
        // --------------------------------------------------

        return services.BuildServiceProvider();
    }
}