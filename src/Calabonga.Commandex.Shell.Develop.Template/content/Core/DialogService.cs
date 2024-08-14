using System.Windows.Controls;
using Calabonga.Commandex.Engine.Base;
using Calabonga.Commandex.Engine.Dialogs;
using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.Commandex.Shell.Core;
using Calabonga.OperationResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Calabonga.Commandex.Shell.Develop.Core;

/// <summary>
/// // Calabonga: Summary required (DialogService 2024-08-03 07:58)
/// </summary>
public class DialogService : IDialogService
{
    private readonly ILogger<DialogService> _logger;

    public DialogService(ILogger<DialogService> logger) => _logger = logger;

    /// <summary>
    /// // Calabonga: Summary required (IDialogService 2024-07-31 05:53)
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    /// <param name="onClosingDialogCallback"></param>
    public OperationEmpty<ExecuteCommandexCommandException> ShowDialog<TView, TViewModel>(Action<TViewModel> onClosingDialogCallback)
        where TView : IView
        where TViewModel : IResult
        => ShowDialogInternal<TView, TViewModel>(onClosingDialogCallback);

    /// <summary>
    /// // Calabonga: Summary required (IDialogService 2024-08-03 07:56)
    /// </summary>
    /// <typeparam name="TView"></typeparam>
    /// <typeparam name="TViewModel"></typeparam>
    public OperationEmpty<ExecuteCommandexCommandException> ShowDialog<TView, TViewModel>() where TView : IDialogView where TViewModel : IDialogResult
        => ShowDialogInternal<TView, TViewModel>();

    public OperationEmpty<ExecuteCommandexCommandException> ShowNotification(string message)
        => ShowDialogInternal(message, Microsoft.Extensions.Logging.LogLevel.Information);

    public OperationEmpty<ExecuteCommandexCommandException> ShowWarning(string message)
        => ShowDialogInternal(message, Microsoft.Extensions.Logging.LogLevel.Warning);

    public OperationEmpty<ExecuteCommandexCommandException> ShowError(string message)
        => ShowDialogInternal(message, Microsoft.Extensions.Logging.LogLevel.Error);

    private string GetTitle(Microsoft.Extensions.Logging.LogLevel type) => type.ToString();

    private OperationEmpty<ExecuteCommandexCommandException> ShowDialogInternal<TView, TViewModel>(Action<TViewModel>? onClosingDialogCallback = null)
        where TView : IView
        where TViewModel : IResult
    {
        EventHandler closeEventHandler = null!;

        var dialog = new DialogWindow { MinWidth = 400, MinHeight = 350 };

        var handler = closeEventHandler;
        closeEventHandler = (sender, _) =>
        {
            var window = (DialogWindow)sender!;
            var userControl = (UserControl)window.Content;
            var viewModel = (TViewModel)userControl.DataContext;
            onClosingDialogCallback?.Invoke(viewModel);
            viewModel.Dispose();
            dialog.Closed -= handler;
        };

        dialog.Closed += closeEventHandler;

        try
        {
            var viewModel = App.Current.Services.GetRequiredService(typeof(TViewModel));
            var control = App.Current.Services.GetRequiredService(typeof(TView));
            var userControl = (UserControl)control;
            userControl.DataContext = viewModel;
            dialog.Content = userControl;

            var viewModelResult = (IResult)viewModel;
            viewModelResult.Owner = dialog;

            var title = viewModelResult.Title;
            dialog.Title = string.IsNullOrEmpty(title) ? "Untitled" : title;

            dialog.ResizeMode = viewModelResult.ResizeMode;
            dialog.SizeToContent = viewModelResult.SizeToContent;
            dialog.WindowStyle = viewModelResult.WindowStyle;

            dialog.InitializeComponent();
            dialog.ShowDialog();

            return Operation.Result();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            return Operation.Error(new ExecuteCommandexCommandException(exception.Message, exception));
        }
    }

    private OperationEmpty<ExecuteCommandexCommandException> ShowDialogInternal(string message, Microsoft.Extensions.Logging.LogLevel type)
    {
        var dialog = new DialogWindow();

        EventHandler closeEventHandler = null!;

        var handler = closeEventHandler;
        closeEventHandler = (_, _) =>
        {
            dialog.Closed -= handler;
        };

        dialog.Closed += closeEventHandler;

        var viewModelResult = new NotificationDialogResult
        {
            Title = message,
            Owner = dialog
        };

        var control = new NotificationDialog
        {
            DataContext = viewModelResult
        };

        dialog.ResizeMode = ((IDialogResult)viewModelResult).ResizeMode;
        dialog.SizeToContent = ((IDialogResult)viewModelResult).SizeToContent;
        dialog.WindowStyle = ((IDialogResult)viewModelResult).WindowStyle;
        dialog.Content = control;
        dialog.Title = GetTitle(type);
        dialog.ShowDialog();

        return Operation.Result();
    }
}