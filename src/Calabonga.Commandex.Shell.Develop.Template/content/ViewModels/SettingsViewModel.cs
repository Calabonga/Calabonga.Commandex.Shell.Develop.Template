using Calabonga.Commandex.Engine.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;

namespace Calabonga.Commandex.Shell.Develop.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    #region property Data

    /// <summary>
    /// Property Data
    /// </summary>
    [ObservableProperty] private string[] _data;

    #endregion

    [RelayCommand]
    private void LoadSettings()
    {
        var path = Path.GetFullPath("../../../commandex.env");

        if (!File.Exists(path))
        {
            return;
        }

        Data = File.ReadAllLines(path);
    }
}
