<!-- Provide an overview of what your template package does and how to get started.
Consider previewing the README before uploading (https://learn.microsoft.com/en-us/nuget/nuget-org/package-readme-on-nuget-org#preview-your-readme). -->

# Calabonga.Commandex.Shell.Develop

## Description

This is a nuget-package [Calabonga.Commandex.Shell.Develop.Template](https://www.nuget.org/packages/Calabonga.Commandex.Shell.Develop.Template) (tools) that install to your Visual Studio a new type of the project. New type project can create a Developer version of the Command Executer (`Calabonga.Commandex`). Witch is created to runs commands of any type for any purposes. For example, to execute a stored procedure or just to copy some files to some destination. And so on... 

## What is Calabonga.Commandex

It's a complex solution with a few repositories:

* **[Calabonga.Commandex.Shell](https://github.com/Calabonga/Calabonga.Commandex.Shell)** → Command Executer or Command Launcher. To run commands of any type for any purpose. For example, to execute a stored procedure or just to copy some files to some destination.

* **[Calabonga.Commandex.Commands](https://github.com/Calabonga/Calabonga.Commandex.Commands)** → Commands for Calabonga.Commandex.Shell that can execute them from unified shell.

* **[Calabonga.Commandex.Shell.Develop.Template](https://github.com/Calabonga/Calabonga.Commandex.Shell.Develop.Template)** → This is a Developer version of the Command Executer (`Calabonga.Commandex`). Witch is created to runs commands of any type for any purposes. For example, to execute a stored procedure or just to co…

* **[Calabonga.Commandex.Engine](https://github.com/Calabonga/Calabonga.Commandex.Engine)** → Engine and contracts library for Calabonga.Commandex. Contracts are using for developing a modules for Commandex Shell.

## How to install template

Nothing is simpler then install this template. Just execute command in `powershell`:

``` powershell
dotnet new install Calabonga.Commandex.Shell.Develop.Template
```

## How to use

This application can only test your Command for Commandex, but almost in a real conditions. How? Please do a few simple steps:

1. Please implement a `ICommandexCommand` interface in WPF Class Library project and add reference to `Calabonga.Commandex.Shell.Develop`.
2. Register your `ICommandexCommand` implementation in the `DependencyContainer.cs`.

    ``` csharp
    internal static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddLogging(options =>
        {
            options.AddSerilog(dispose: true);
            options.AddDebug();
        });

        services.AddSingleton<DefaultDialogView>();
        services.AddSingleton<MainWindow>();
        services.AddSingleton<ViewModels.MainWindowsViewModel>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<IAppSettings>(_ => App.Current.Settings);
        services.AddSingleton<ISettingsReaderConfiguration, DefaultSettingsReaderConfiguration>();

        // dialogs and wizard
        services.AddTransient<IWizardView, Wizard>();
        services.AddTransient<IDialogService, DialogService>();
        services.AddTransient(typeof(IWizardManager<>), typeof(WizardManager<>));

        // --------------------------------------------------
        // 1. Attach command definition from your project where Commandex.Command implemented.
        // 2. Then uncomment line below and add your command type.
        // services.AddDefinitions(typeof(WelcomeAppDefinition)); // <-- uncomment line and register your command here
        // --------------------------------------------------

        return services.BuildServiceProvider();
    }
    ```


3. Inject your command implementation into `MainWindowsViewModel` as `ICommandexCommand`.
    ``` csharp
    public partial class MainWindowsViewModel : ViewModelBase
    {
        private readonly IDialogService _dialogService;

        public MainWindowsViewModel(IDialogService dialogService, IAppSettings settings)
        {
            Title = $"Commandex Shell Emulator for Easy developing ({settings.CommandsPath})";
            Version = "1.0.0-rc.7";
            _dialogService = dialogService;
        }

        [ObservableProperty]
        private string _version;

        /// <summary>
        /// Executes MVVM button action
        /// </summary>
        [RelayCommand]
        private Task ExecuteAsync()
        {
            _dialogService.ShowNotification("You do not attach your ICommandexCommand yet. " +
                                            "Please add your component definition in the DependencyContainer.cs file.");
            return Task.CompletedTask;
        }
    }
    ```

4. Use your injected instance in `ExecuteAsync()` method to execute command as shown above.
5. If you everything do correctly, than after button click on the form your command will come executed.

## Screenshot

![image](https://github.com/user-attachments/assets/9393d2a6-fbf8-40ff-a3df-ee1b185f705e)

## Ingredients

WPF, MVVM, CommunityToolkit, AppDefinitions, etc.

## Versions history 


### v1.0.0-rc.11 2024-09-11

* Engine-nuget updated as dependency.

### v1.0.0-rc.10 2024-09-11

* Nuget dependencies updated.
* `AppSettings` update with parameter `NugetFeedUrl` moved into configuration file (.env).
* `NugetFeedUrl` default feed URL is https://api.nuget.org/v3/index.json.
* Default folder for logs set up to *../bin/{Debug|Release}/logs/** folder.
* `SettingsFinder` marked as `static`.

### v1.0.0-rc.9 2024-09-07

* Nuget dependencies updated.

### v1.0.0-rc.7

* Some instructions were updated.
* Template default name parameter added.
* Nuget dependencies updated.

### v1.0.0-rc.6

* Abstraction for configuration reader created.
* Nuget dependencies updated.

### v1.0.0-rc.3

* Wizard component added from Engine
* Nuget dependencies updated.

### v1.0.0-beta.15 2024-08-14

* Nuget dependencies updated.

### v1.0.0-beta.12 2024-08-07

* `SettingsFinder` copied from the main Shell project.
* Shell settings as `IAppSettings` injected to `MainWindowViewModel`.
* `commandex.env` file created with default parameters.
* Nuget dependencies updated.

### v1.0.0-beta.9 2024-08-05

* Core from original Shell updated.
* Nuget dependencies updated.
* `MainWindow` updated with new welcome message (instructions).
* 
### v1.0.0-beta.5 2024-08-01

* `GitInfo` nuget package removed because throwing error when git not used.
* Readme.md updated with screenshot and som code snippets.


### v1.0.0-beta.1 2024-08-01

* First commit
