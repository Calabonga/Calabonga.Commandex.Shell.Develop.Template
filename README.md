# Calabonga.Commandex.Shell.Develop

## Description

This is a nuget-package [Calabonga.Commandex.Shell.Develop.Template](https://www.nuget.org/packages/Calabonga.Commandex.Shell.Develop.Template) (tools) that install to your Visual Studio a new type of the project. New type project can create a Developer version of the Command Executer (`Calabonga.Commandex`). Witch is created to runs commands of any type for any purposes. For example, to execute a stored procedure or just to copy some files to some destination. And so on... 

## What is Calabonga.Commandex

The `Calabonga.Commandex` - This is an application on WPF-platform built with CommunityToolkit.MVVM for modules (plugins) using: launch and execute.

What is the `Calabonga.Commandex` can:
* Find a modules `.dll` (plugins) in the folder you set up.
* Launch or execute modules `.dll` (plughis) from GUI.
* Get the results of the module's (plugis) work after they completed.

It's a complex solution with a few repositories:

* **[Calabonga.Commandex.Shell](https://github.com/Calabonga/Calabonga.Commandex.Shell)** →  Command Executer or Command Launcher. To run commands of any type for any purpose. For example, to execute a stored procedure or just to copy some files to some destination.
* **[Calabonga.Commandex.Commands](https://github.com/Calabonga/Calabonga.Commandex.Commands)** →  Commands for Calabonga.Commandex.Shell that can execute them from unified shell.
* **[Calabonga.Commandex.Shell.Develop.Template](https://github.com/Calabonga/Calabonga.Commandex.Shell.Develop.Template)** →  (`Tool Template`) This is a Developer version of the Command Executer Shell (`Calabonga.Commandex`). Which is created to runs commands of any type for any purposes. For example, to execute a stored procedure or just to co…
* **[Calabonga.Commandex.Engine](https://github.com/Calabonga/Calabonga.Commandex.Engine)** →  Engine and contracts library for Calabonga.Commandex. Contracts are using for developing a modules for Commandex Shell.
* **[Calabonga.Commandex.Engine.Processors](https://github.com/Calabonga/Calabonga.Commandex.Engine.Processors)** →  Results Processors for Calabonga.Commandex.Shell commands execution results. This is an extended version of the just show string in the notification dialog.
* **[Calabonga.CommandexCommand.Template](https://github.com/Calabonga/Calabonga.CommandexCommand.Template)** →  (`Tool Template`) This is a template of the project to create a Command for Commandex. Just install this nuget as a template for Visual Studio (Rider or dotnet CLI) and then you can create a DialogCommand faster.

## How to install template

Nothing is simpler then install this template. Just execute command in `powershell`:

``` powershell
dotnet new install Calabonga.Commandex.Shell.Develop.Template
```

## How to use

This application tests your Command for Commandex in almost real conditions. Commands are discovered automatically: the shell scans its own output folder for assemblies and registers every `AppDefinition` / `ICommandexCommand` it finds. A few simple steps:

1. Install the template and scaffold a developer shell:

    ``` powershell
    dotnet new install Calabonga.Commandex.Shell.Develop.Template
    dotnet new wpfshell -n Commandex.Developer.Shell
    ```

2. Implement `ICommandexCommand` (and its `AppDefinition`) in a WPF Class Library project, then add a project reference to it from `Commandex.Developer.Shell`. Its DLL now lands next to the shell on build.

3. Set a real path in `commandex.env`:

    ``` ini
    COMMANDS_FOLDER="C:\Path\To\Your\Commands"
    ```

4. Build and run the developer shell. `RegisterCommandsDefinitions()` (in `Engine/ServiceCollectionExtension.cs`) scans the output folder and auto-registers all commands found, including the bundled `SampleCommand`. Pick one on the **Executor** tab and press **Execute**.

5. The result is passed through `IResultProcessor` (`AdvancedResultProcessor` from `Calabonga.Commandex.Engine.Processors`), and a toast notification shows the outcome. Logs are written to `logs/local-*.log` (configured in `App`).

**Fallback — manual registration.** If you do not want the folder scan, register the definition explicitly in `Engine/DependencyContainer.cs`:

``` csharp
// register all commands
services.AddDefinitions(typeof(YourAppDefinition));
```

## Screenshot

v2.8.1

<img width="1183" height="774" alt="image" src="https://github.com/user-attachments/assets/c77116d5-198e-4438-8ab9-5b89564a61e7" />


v1.0.0

![image](https://github.com/user-attachments/assets/9393d2a6-fbf8-40ff-a3df-ee1b185f705e)

## Ingredients

WPF, MVVM, CommunityToolkit, AppDefinitions, etc.

## Видео (Video)

В основном репозитории [Calabonga.Commandex.Shell](https://github.com/Calabonga/Calabonga.Commandex.Shell) есть несколько видео с инструкциями и разъяснениями, как использовать Commandex. А также видео о том, какие типы команд существуют и как для Commandex создавать команды разных типов.
