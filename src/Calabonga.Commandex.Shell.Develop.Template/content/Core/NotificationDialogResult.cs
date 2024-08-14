using Calabonga.Commandex.Engine.Dialogs;
using System.Windows;

namespace Calabonga.Commandex.Shell.Core;

/// <summary>
/// // Calabonga: Summary required (NotificationDialogResult 2024-07-29 04:07)
/// </summary>
public partial class NotificationDialogResult : DefaultDialogResult
{
    /// <summary>
    /// Default value <see cref="WindowStyle.ToolWindow"/>
    /// </summary>
    public override WindowStyle WindowStyle => WindowStyle.ToolWindow;

    public override void Dispose() { }
}