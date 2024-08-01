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

## How to use

This application can only test your Command for Commandex, but almost in a real conditions. How? Please do a few simple steps:

0. Please implement a `ICommandexCommand` interface in WPF Class Library project and add reference to `Calabonga.Commandex.Shell.Develop`.
1. Register your `ICommandexCommand` implementation in the `DependencyContainer.cs`.
2. Inject your command implementation into `MainWindowsViewModel` as `ICommandexCommand`.
3. Use your injected instance in `ExecuteAsync()` method to execute command.
4. If you everything do correctly, than after button click on the form your command will come executed.

## Ingredients

WPF, MVVM, CommunityToolkit, etc.

## Versions history 

### v1.0.0-beta.1 2024-08-01

* First commit