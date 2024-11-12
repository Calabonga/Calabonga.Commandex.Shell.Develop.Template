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
        Title = $"Commandex Shell Emulator for Easy developing ({settings.CommandsPath})";
        Version = "1.4.2";
        _dialogService = dialogService;
    }

    [ObservableProperty]
    private string _version;

    /// <summary>
    /// Executes MVVM button action
    /// </summary>
    [RelayCommand]
    private Task ExecuteAsync()
    {
        _dialogService.ShowNotification("You do not attach your ICommandexCommand yet.");
        return Task.CompletedTask;
    }
}
