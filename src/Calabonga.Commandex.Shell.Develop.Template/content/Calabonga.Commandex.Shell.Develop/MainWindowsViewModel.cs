using Calabonga.Commandex.Contracts;
using Calabonga.Commandex.Shell.Develop.Engine;
using CommunityToolkit.Mvvm.Input;

namespace Calabonga.Commandex.Shell.Develop
{
    public partial class MainWindowsViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        public MainWindowsViewModel(
            IVersionService versionService,
            IDialogService dialogService)
        {
            Title = $"Commandex Shell Developer v.{versionService.Version}";
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
