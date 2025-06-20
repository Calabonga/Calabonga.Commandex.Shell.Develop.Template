using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Settings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Calabonga.Commandex.Shell.Develop.ViewModels;

/// <summary>
/// ViewModel for MainWindow View.
/// </summary>
public partial class MainWindowsViewModel : ViewModelBase, IDisposable
{
    private readonly ICommandexCommand _command;
    private readonly IResultProcessor _resultProcessor;
    private readonly DispatcherTimer _statusTimer;

    public MainWindowsViewModel(
        ICommandexCommand command,
        IResultProcessor resultProcessor,
        IAppSettings settings)
    {
        _statusTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
        _command = command;
        _resultProcessor = resultProcessor;
        Title = $"Commandex Shell Emulator for Easy developing ({settings.CommandsPath})";
        Version = "2.3.0";
        Message = _command.DisplayName;

        InitTimer();
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

    #region property Status

    /// <summary>
    /// Property Status
    /// </summary>
    [ObservableProperty] private string _status;

    #endregion

    #region property StatusBrush

    /// <summary>
    /// Property StatusBrush
    /// </summary>
    [ObservableProperty] private SolidColorBrush _statusBrush;

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
            Status = "Failed";
            StatusBrush = new SolidColorBrush(Colors.Red);
            StartTimer();
            return;
        }

        _resultProcessor.ProcessCommand(_command);
        Status = "Success";
        StatusBrush = new SolidColorBrush(Colors.ForestGreen);
        StartTimer();
    }

    #endregion

    #region private helpers

    private void StartTimer()
    {
        _statusTimer.IsEnabled = true;
        _statusTimer.Start();
    }

    private void InitTimer()
    {
        _statusTimer.Tick += OnTimerTick;
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        Status = string.Empty;
        StatusBrush = new SolidColorBrush(Colors.Transparent);
        _statusTimer.Stop();
        _statusTimer.IsEnabled = false;
    }

    #endregion

    public void Dispose()
    {
        _statusTimer.Tick -= OnTimerTick;
        _command.Dispose();
    }
}
