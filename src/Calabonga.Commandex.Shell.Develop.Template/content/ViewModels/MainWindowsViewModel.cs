using Calabonga.Commandex.Engine;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.Shell.Develop.ViewModels;

public partial class MainWindowsViewModel : ViewModelBase
{
    private readonly IDialogService _dialogService;

    public MainWindowsViewModel(
        IDialogService dialogService,
        IAppSettings settings)
    {
        Title = $"Commandex Shell Emulator for Easy developing ({settings.CommandsPath})";
        _dialogService = dialogService;
    }


    [RelayCommand]
    private Task ExecuteAsync()
    {
        _dialogService.ShowNotification("You do not attach your ICommandexCommand yet.");
        return Task.CompletedTask;
    }
}