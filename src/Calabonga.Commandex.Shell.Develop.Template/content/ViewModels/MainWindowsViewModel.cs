using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Settings;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.Shell.Develop.ViewModels;

/// <summary>
/// ViewModel for MainWindow View.
/// </summary>
public partial class MainWindowsViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;

    public MainWindowsViewModel(IDialogService dialogService, IAppSettings settings)
    {
        _dialogService = dialogService;
        Title = $"Commandex Shell Emulator for Easy developing ({settings.CommandsPath})";
        Version = "2.0.0-beta.1 (NET9.0)";
        Message = "You do not attach your ICommandexCommand yet.";
    }

    [ObservableProperty]
    private string _version;

    [ObservableProperty]
    private string _message;

    /// <summary>
    /// Executes MVVM button action
    /// </summary>
    [RelayCommand]
    private Task ExecuteAsync()
    {
        _dialogService.ShowNotification(Message);
        return Task.CompletedTask;
    }
}
