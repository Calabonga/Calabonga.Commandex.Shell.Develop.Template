using System.Collections.ObjectModel;
using Calabonga.Commandex.Engine.Exceptions;
using Calabonga.Commandex.Engine.Extensions;
using Calabonga.Commandex.Engine.Wizards;
using Calabonga.Commandex.Engine.Wizards.Messages;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Calabonga.Commandex.Shell.Develop.Core;

/// <summary>
/// // Calabonga: Summary required (WizardManager 2024-08-13 08:01)
/// </summary>
public class WizardManager<TPayload> : IWizardManager<TPayload>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly List<IWizardStep<IWizardStepView, IWizardStepViewModel<TPayload>>> _internalSteps;

    public WizardManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _internalSteps = _serviceProvider
            .GetServices<IWizardStep<IWizardStepView, IWizardStepViewModel<TPayload>>>()
            .OrderBy(x => x.OrderIndex)
            .ToList();
    }

    /// <summary>
    /// Deactivates current Step if it exists and Activates the next Step.
    /// </summary>
    /// <param name="wizardContext"></param>
    public void ActivateStep(WizardContext<TPayload> wizardContext)
    {
        if (IsCanDeactivatePreviousStep(wizardContext))
        {
            return;
        }

        _internalSteps.DeactivateAll();

        ResolveFromContainer(wizardContext);

        var steps = new ObservableCollection<IWizardStep>(_internalSteps);
        var activeStep = GetActiveStep();

        WeakReferenceMessenger.Default.Send(new ManagerStepActivatedMessage(steps, activeStep));
    }

    private bool IsCanDeactivatePreviousStep(WizardContext<TPayload> wizardContext)
    {
        var activeStep = GetActiveStep();
        if (activeStep is null)
        {
            return false;
        }

        var activeView = (IWizardStepView)activeStep.Content;
        var activeViewModel = (IWizardStepViewModel<TPayload>)activeView.DataContext!;
        var canLeave = activeViewModel.HasErrors;
        if (canLeave)
        {
            return true;
        }

        activeViewModel.OnLeave(wizardContext.Payload);
        return false;
    }

    /// <summary>
    /// // Calabonga: Summary required (WizardManager 2024-08-13 05:16)
    /// </summary>
    /// <param name="wizardContext"></param>
    /// <exception cref="WizardInvalidOperationException"></exception>
    private void ResolveFromContainer(WizardContext<TPayload> wizardContext)
    {
        var activeStep = GetStep(wizardContext);
        var types = activeStep.GetStepTypes();
        var viewType = _serviceProvider.GetService(types[0]);
        var viewModelType = _serviceProvider.GetService(types[1]);
        if (viewType is not IWizardStepView view)
        {
            throw new WizardInvalidOperationException($"Unable to get View object from {nameof(IWizardStepView)}");
        }

        if (viewModelType is not IWizardStepViewModel<TPayload> viewModel)
        {
            throw new WizardInvalidOperationException($"Unable to get View object from {nameof(IWizardStepViewModel)}");
        }

        viewModel.OnEnter(wizardContext.Payload);
        view.DataContext = viewModel;
        activeStep.HasErrors = viewModel.HasErrors;
        activeStep.Activate(view);
    }

    private IWizardStep? GetActiveStep()
    {
        var step = _internalSteps.Find(x => x.IsActive);
        if (step is null)
        {
            return step;
        }

        var index = _internalSteps.IndexOf(step);
        ((WizardStep)step).SetLast(index == _internalSteps.Count - 1);
        return step;
    }

    private IWizardStep<IWizardStepView, IWizardStepViewModel<TPayload>> GetStep(WizardContext<TPayload> wizardContext)
    {
        var step = _internalSteps[wizardContext.StepIndex];
        var index = _internalSteps.IndexOf(step);
        ((WizardStep)step).SetLast(index == _internalSteps.Count - 1);
        return step;
    }
}