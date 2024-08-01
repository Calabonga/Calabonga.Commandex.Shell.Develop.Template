namespace Calabonga.Commandex.Shell.Develop.Engine;

public interface IVersionService
{
    string Version { get; }
}

public class VersionService : IVersionService
{
    public string Version => $"{ThisAssembly.Git.SemVer.Major}.{ThisAssembly.Git.SemVer.Minor}.{ThisAssembly.Git.SemVer.Patch}";
}