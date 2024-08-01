using Calabonga.Commandex.Engine;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.Shell.Develop
{
    public partial class MainWindowsViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        public MainWindowsViewModel(IDialogService dialogService)
        {
            Title = $"Commandex Shell Emulator for Developer";
            _dialogService = dialogService;
        }

        [RelayCommand]
        private Task ExecuteAsync()
        {
            _dialogService.ShowNotification("You do not attach your ICommandexCommand yet.");
            return Task.CompletedTask;
        }
    }
}
