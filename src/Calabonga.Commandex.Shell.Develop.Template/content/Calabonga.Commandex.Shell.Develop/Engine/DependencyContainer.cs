using Calabonga.Commandex.Engine;
using Calabonga.Commandex.Shell.Develop.Core;
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

        services.AddSingleton(typeof(DefaultDialogResult<>));
        services.AddSingleton<DefaultDialogView>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<MainWindowsViewModel>();
        services.AddSingleton<IDialogService, DialogService>();

        // --------------------------------------------------
        // Attach your definition from your project with your
        // Commandex.Command implementation, uncomment line below and add your command type.
        // services.AddDefinitions(typeof(YourCommandDefinition)); // <-- here should be your Command
        // --------------------------------------------------

        return services.BuildServiceProvider();
    }
}