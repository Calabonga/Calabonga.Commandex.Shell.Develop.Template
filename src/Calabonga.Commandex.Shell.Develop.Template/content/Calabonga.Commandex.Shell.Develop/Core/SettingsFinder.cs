using DotNetEnv;

namespace Calabonga.Commandex.Shell.Develop.Core;

/// <summary>
/// // Calabonga: Summary required (SettingsFinder 2024-08-07 07:52)
/// </summary>
internal class SettingsFinder
{
    public static AppSettings Configure()
    {
        Env.Load("commandex.env", LoadOptions.TraversePath());

        var appSettings = new AppSettings
        {
            CommandsPath = Environment.GetEnvironmentVariable("COMMANDS_FOLDER") ?? throw new ArgumentNullException($"COMMANDS_FOLDER"),
            ShowSearchPanelOnStartup = bool.Parse(Environment.GetEnvironmentVariable("SHOW_SEARCH_PANEL_ONSTARTUP") ?? "false"),
            ArtifactsFolderName = Environment.GetEnvironmentVariable("ARTIFACTS_FOLDER_NAME") ?? "Artifacts"
        };

        return appSettings;
    }
}