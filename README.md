<!-- Provide an overview of what your template package does and how to get started.
Consider previewing the README before uploading (https://learn.microsoft.com/en-us/nuget/nuget-org/package-readme-on-nuget-org#preview-your-readme). -->

# Calabonga.Commandex.Shell.Develop

## Description

This is a Developer version of the Command Executer (`Calabonga.Commandex`). Witch is created to runs commands of any type for any purposes. For example, to execute a stored procedure or just to copy some files to some destination. And so on... 

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
    internal static class DependencyContainer
    {
        /// <summary>
        /// Executes some registrations for Commandex. You can use this register your CommandexCommand for testing
        /// </summary>
        /// <returns></returns>
        internal static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();


            services.AddLogging(options =>
            {
                options.AddSerilog(dispose: true);
                options.AddDebug();
            });

            services.AddSingleton(typeof(DefaultDialogResult<>));
            services.AddSingleton<DefaultDialogView>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<MainWindowsViewModel>();
            services.AddSingleton<IDialogService, DialogService>();

            // --------------------------------------------------
            // Attach your definition from your project with your
            // Commandex.Command implementation, uncomment line below and add your command type.
            // services.AddDefinitions(typeof(YourCommandDefinition)); 
            // --------------------------------------------------

            return services.BuildServiceProvider();
        }
    }
    ```


3. Inject your command implementation into `MainWindowsViewModel` as `ICommandexCommand`.
    ``` csharp
    public partial class MainWindowsViewModel : ViewModelBase
    {
        private readonly ICommandexCommand _command; // ← create private field

        public MainWindowsViewModel(ICommandexCommand command) // ← inject base interface
        {
            _command = command; // ← do not forget this
            Title = $"Commandex Shell Developer Emulator";
        }

        [RelayCommand]
        private Task ExecuteAsync()
        {
            _command.ShowDialogAsync(); // ← show command dialog

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

### v1.0.0-beta.9 2024-08-05

* Core from original Shell updated.
* Nuget dependencies updated.
* MainWindow updated with new welcome message (instructions).
* 
### v1.0.0-beta.5 2024-08-01

* `GitInfo` nuget package removed because throwing error when git not used.
* Readme.md updated with screenshot and som code snippets.


### v1.0.0-beta.1 2024-08-01

* First commit
