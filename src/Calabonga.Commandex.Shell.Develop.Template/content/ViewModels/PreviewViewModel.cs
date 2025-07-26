using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.ToastNotifications;
using Calabonga.Commandex.Engine.Zones;
using Calabonga.Commandex.Shell.Develop.Zones;
using Calabonga.Utils.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Calabonga.Commandex.Shell.Develop.ViewModels;

public partial class PreviewViewModel : ZoneViewModelBase, IPreviewViewModel
{

    private readonly IResultProcessor _resultProcessor;
    private readonly INotificationManager _notificationManager;
    public PreviewViewModel(IEnumerable<ICommandexCommand> commands, INotificationManager notificationManager, IResultProcessor resultProcessor)
    {
        _notificationManager = notificationManager;
        _resultProcessor = resultProcessor;
        Commands = new ObservableCollection<ICommandexCommand>(commands);
        Version = GetEngineVersion();
        if (Commands.Any())
        {
            SelectedCommand = Commands.First();
            IsMoreThanOneCommandFound = Commands.Count > 1;
        }

    }

    #region property IsMoreThanOneCommandFound

    /// <summary>
    /// Property IsMoreThanOneCommandFound
    /// </summary>
    [ObservableProperty] private bool _isMoreThanOneCommandFound;

    #endregion

    #region property Message

    /// <summary>
    /// Property Message
    /// </summary>
    [ObservableProperty] private string _message;

    #endregion

    #region property SelectedCommand

    /// <summary>
    /// Property SelectedCommand
    /// </summary>
    [ObservableProperty]
    private ICommandexCommand? _selectedCommand;

    #endregion

    #region property Commands

    /// <summary>
    /// Property Commands
    /// </summary>
    [ObservableProperty] private ObservableCollection<ICommandexCommand> _commands;

    #endregion

    #region property Version

    /// <summary>
    /// Property Version
    /// </summary>
    [ObservableProperty] private string _version;

    #endregion

    #region command ExecuteCommand
    private bool CanExecuteCommand => SelectedCommand is not null;

    /// <summary>
    /// Executes MVVM button action and process result from command.
    /// Do not make any changes to this method if you are not sure.
    /// </summary>
    [RelayCommand(CanExecute = nameof(CanExecuteCommand))]
    private async Task ExecuteAsync()
    {
        IsBusy = true;

        var result = await SelectedCommand!.ExecuteCommandAsync();
        if (!result.Ok)
        {
            IsBusy = false;
            _notificationManager.Show(NotificationManager.CreateErrorToast(result.Error.Message), "NotificationZone");

            return;
        }

        IsBusy = false;

        _resultProcessor.ProcessCommand(SelectedCommand);

        if (SelectedCommand.IsPushToShellEnabled)
        {
            var commandResult = SelectedCommand.GetResult();
            if (commandResult is null)
            {
                _notificationManager.Show(NotificationManager.CreateErrorToast("Not result from command"), "NotificationZone");

                return;
            }

            _notificationManager.Show(NotificationManager.CreateSuccessToast(commandResult.ToString() ?? "Result From Command is NULL", "Result from command"), "NotificationZone");

            return;
        }
        _notificationManager.Show(NotificationManager.CreateSuccessToast("Command parameter IsPushToShellEnabled is FALSE", "Result from command"), "NotificationZone");
    }

    #endregion

    #region privates

    /// <summary>
    /// Extracts Engine version
    /// </summary>
    /// <returns></returns>
    private string GetEngineVersion()
    {
        var dll = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(x => x.GetName().Name?.Contains("Calabonga.Commandex.Engine") == true);

        if (dll is null)
        {
            return "0.0.0";
        }

        var semver = dll.GetSemanticVersion();
        if (string.IsNullOrEmpty(semver.Prerelease))
        {
            return $"{semver.Major}.{semver.Minor}.{semver.Patch}";
        }

        return $"{semver.Major}.{semver.Minor}.{semver.Patch} {semver.Prerelease}";
    }

    #endregion

    partial void OnSelectedCommandChanged(ICommandexCommand? value)
    {
        Message = value?.DisplayName ?? "Not selected";
        ExecuteCommand.NotifyCanExecuteChanged();
    }
}
