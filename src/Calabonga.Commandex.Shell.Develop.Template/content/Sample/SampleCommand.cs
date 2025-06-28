using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Commands;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.OperationResults;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Calabonga.Commandex.Shell.Develop.Sample;

/// <summary>
/// Sample command definition for DI-container
/// </summary>
public class SampleCommandDefinition : AppDefinition
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ICommandexCommand, SampleCommand>();
    }
}

/// <summary>
/// Commandex Command
/// </summary>
public class SampleCommand : EmptyCommandexCommand
{
    private readonly IDialogService _dialogService;

    public SampleCommand(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    public override string CopyrightInfo => "Calabonga SOFT";

    public override string DisplayName => "Empty Command Sample";

    public override string Description => "For demo purposes only. Replace this SampleCommand with your own.";

    public override string Version => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0";

    public override Task<OperationEmpty<ExecuteCommandexCommandException>> ExecuteCommandAsync()
    {
        _dialogService.ShowNotification($"I'm a [{DisplayName}] from {CopyrightInfo}.\nThis command created just: {Description}");
        return Task.FromResult<OperationEmpty<ExecuteCommandexCommandException>>(Operation.Result());
    }
}
