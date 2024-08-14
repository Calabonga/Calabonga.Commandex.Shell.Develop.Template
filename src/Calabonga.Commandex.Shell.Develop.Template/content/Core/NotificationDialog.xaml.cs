using System.Windows;
using System.Windows.Controls;
using Calabonga.Commandex.Engine.Dialogs;

namespace Calabonga.Commandex.Shell.Develop.Core;

/// <summary>
/// Interaction logic for NotificationDialog.xaml
/// </summary>
public partial class NotificationDialog : UserControl, IDialogView
{
    public NotificationDialog()
    {
        InitializeComponent();
    }

    public object ViewModel => DataContext;

    private void OnButtonOkClicked(object sender, RoutedEventArgs e)
    {
        var window = Parent as Window;
        if (window is not null)
        {
            window.DialogResult = true;
        }
        window?.Close();
    }
}