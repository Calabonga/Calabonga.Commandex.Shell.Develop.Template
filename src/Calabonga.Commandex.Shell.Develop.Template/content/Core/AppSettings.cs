using Calabonga.Commandex.Engine.Settings;

namespace Calabonga.Commandex.Shell.Develop.Core;

/// <summary>
/// Application settings imported from .env-file with parameters.
/// </summary>
public class AppSettings : IAppSettings
{
    /// <summary>
    /// Where Commandex will search the commands
    /// </summary>
    public required string CommandsPath { get; set; }

    /// <summary>
    /// If True then search bar on the top of the commands list will be visible by default.
    /// </summary>
    public bool ShowSearchPanelOnStartup { get; set; }

    /// <summary>
    /// Artifacts folder name
    /// </summary>
    public required string ArtifactsFolderName { get; set; }
}