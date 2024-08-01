﻿using System.Windows.Controls;
using Calabonga.Commandex.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.Shell.Develop.Engine;

/// <summary>
/// DialogService implementation
/// </summary>
public class DialogService : IDialogService
{
    public void ShowDialog<TView, TViewModel>(Action<TViewModel> onClosingDialogCallback)
        where TView : IDialogView
        where TViewModel : IDialogResult
    {
        EventHandler closeEventHandler = null!;

        var dialog = new DialogWindow();

        var handler = closeEventHandler;
        closeEventHandler = (sender, _) =>
        {
            var window = (DialogWindow)sender!;
            var userControl = (UserControl)window.Content;
            var viewModel = ((TViewModel)userControl.DataContext);
            onClosingDialogCallback(viewModel);
            dialog.Closed -= handler;
        };

        dialog.Closed += closeEventHandler;

        var viewModel = App.Current.Services.GetRequiredService(typeof(TViewModel));
        var control = App.Current.Services.GetRequiredService(typeof(TView));
        var userControl = ((UserControl)control);
        userControl.DataContext = viewModel;
        dialog.Content = userControl;
        dialog.Title = ((IDialogResult)viewModel).DialogTitle;

        dialog.ShowDialog();
        dialog.InitializeComponent();

    }

    /// <summary>
    /// Shows dialogs with simple message
    /// </summary>
    /// <param name="message"></param>
    public void ShowNotification(string message)
    {
        ShowDialog(message, LogLevel.Notification);
    }

    public void ShowWarning(string message)
    {
        ShowDialog(message, LogLevel.Warning);
    }

    public void ShowError(string message)
    {
        ShowDialog(message, LogLevel.Error);
    }

    private string GetTitle(LogLevel type)
    {
        return type switch
        {
            LogLevel.Notification => "Уведомление",
            LogLevel.Warning => "Предупреждение",
            LogLevel.Error => "Ошибка",
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    private void ShowDialog(string message, LogLevel type)
    {
        var dialog = new DialogWindow();

        EventHandler closeEventHandler = null!;

        var handler = closeEventHandler;
        closeEventHandler = (s, e) =>
        {
            dialog.Closed -= handler;
        };

        dialog.Closed += closeEventHandler;

        var control = new NotificationDialog
        {
            DataContext = new NotificationDialogViewModel
            {
                Title = message
            }
        };

        dialog.Content = control;

        dialog.Title = GetTitle(type);
        dialog.Content = control;

        dialog.ShowDialog();
    }
}