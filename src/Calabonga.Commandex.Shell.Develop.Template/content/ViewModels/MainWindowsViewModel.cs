using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Settings;
using Calabonga.Commandex.Engine.ToastNotifications;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.Shell.Develop.ViewModels;

/// <summary>
/// ViewModel for MainWindow View.
/// </summary>
public partial class MainWindowsViewModel : ViewModelBase, IDisposable
{
    private readonly ICommandexCommand _command;
    private readonly IResultProcessor _resultProcessor;
    private readonly INotificationManager _notificationManager;

    public MainWindowsViewModel(
        ICommandexCommand command,
        IResultProcessor resultProcessor,
        IAppSettings settings, INotificationManager notificationManager)
    {
        _command = command;
        _resultProcessor = resultProcessor;
        _notificationManager = notificationManager;
        Title = $"Commandex Shell Emulator ({settings.CommandsPath})";
        Version = GetEngineVersion();
        Message = _command.DisplayName;

        Metadata = _command;
    }
    #region property Message

    /// <summary>
    /// Property Message
    /// </summary>
    [ObservableProperty] private string _message;

    #endregion

    #region property Version

    /// <summary>
    /// Property Version
    /// </summary>
    [ObservableProperty] private string _version;

    #endregion

    #region property Metadata

    /// <summary>
    /// Property Metadata
    /// </summary>
    [ObservableProperty] private ICommandexCommand _metadata;

    #endregion

    #region command ExecuteCommand

    /// <summary>
    /// Executes MVVM button action and process result from command.
    /// Do not make any changes to this method if you are not sure.
    /// </summary>
    [RelayCommand]
    private async Task ExecuteAsync()
    {
        var result = await _command.ExecuteCommandAsync();
        if (!result.Ok)
        {
            Message = result.Error.Message;
            _notificationManager.Show(NotificationManager.CreateErrorToast(Message), "NotificationZone");
            return;
        }

        _notificationManager.Show(NotificationManager.CreateSuccessToast("Successfully executed"), "NotificationZone");
        _resultProcessor.ProcessCommand(_command);
    }

    #endregion

    public void Dispose()
    {
        _command.Dispose();
    }

    /// <summary>
    /// Extracts Engine version
    /// </summary>
    /// <returns></returns>
    private string GetEngineVersion()
    {
        var dll = AppDomain.CurrentDomain
            .GetAssemblies()
            .FirstOrDefault(x => x.GetName().Name?.Contains("Calabonga.Commandex.Engine") == true);

        if (dll is not null)
        {
            return dll.GetName().Version?.ToString(3) ?? "0.0.0";
        }

        return "0.0.0";
    }
}
