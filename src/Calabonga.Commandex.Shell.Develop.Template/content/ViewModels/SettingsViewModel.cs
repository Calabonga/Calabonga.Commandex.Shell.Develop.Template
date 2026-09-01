using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Settings;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Calabonga.Commandex.Shell.Develop.ViewModels;

public sealed partial class SettingsViewModel : ViewModelBase
{
    #region property Data

    /// <summary>
    /// Property Data
    /// </summary>
    [ObservableProperty] private string[]? _data;

    #endregion

    public SettingsViewModel(IAppSettings settings)
        => Data =
        [
            $"COMMANDS_FOLDER = {settings.CommandsPath}",
            $"SETTINGS_FOLDER = {settings.SettingsPath}",
            $"SHOW_SEARCH_PANEL_ONSTARTUP = {settings.ShowSearchPanelOnStartup}",
            $"ARTIFACTS_FOLDER_NAME = {settings.ArtifactsFolderName}",
            $"NUGET_FEED_URL = {settings.NugetFeedUrl}",
        ];
}
