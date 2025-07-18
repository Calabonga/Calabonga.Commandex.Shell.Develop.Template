using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.ToastNotifications;
using Calabonga.Commandex.Engine.Zones;
using Calabonga.Commandex.Shell.Develop.Zones;
using Calabonga.Utils.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.Shell.Develop.ViewModels;

public partial class PreviewViewModel : ZoneViewModelBase, IPreviewViewModel
{
    private readonly ICommandexCommand _command;

    private readonly IResultProcessor _resultProcessor;
    private readonly INotificationManager _notificationManager;
    public PreviewViewModel(ICommandexCommand command, INotificationManager notificationManager, IResultProcessor resultProcessor)
    {
        _command = command;
        _notificationManager = notificationManager;
        _resultProcessor = resultProcessor;
        Message = _command.DisplayName;
        Metadata = _command;
        Version = GetEngineVersion();
    }

    #region property Message

    /// <summary>
    /// Property Message
    /// </summary>
    [ObservableProperty] private string _message;

    #endregion

    #region property Metadata

    /// <summary>
    /// Property Metadata
    /// </summary>
    [ObservableProperty] private ICommandexCommand _metadata;

    #endregion

    #region property Version

    /// <summary>
    /// Property Version
    /// </summary>
    [ObservableProperty] private string _version;

    #endregion

    #region command ExecuteCommand

    /// <summary>
    /// Executes MVVM button action and process result from command.
    /// Do not make any changes to this method if you are not sure.
    /// </summary>
    [RelayCommand]
    private async Task ExecuteAsync()
    {
        IsBusy = true;

        var result = await _command.ExecuteCommandAsync();
        if (!result.Ok)
        {
            IsBusy = false;
            _notificationManager.Show(NotificationManager.CreateErrorToast(result.Error.Message), "NotificationZone");

            return;
        }

        IsBusy = false;

        _resultProcessor.ProcessCommand(_command);

        if (_command.IsPushToShellEnabled)
        {
            var commandResult = _command.GetResult();
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
}
