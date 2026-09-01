# CLAUDE.md

Guidance for Claude Code (claude.ai/code) when working in the **`Calabonga.Commandex.Shell.Develop.Template`** repository.

> Дополнительные правила — в [`.claude/rules/code-styles.md`](rules/code-styles.md) (стиль C#) и [`.claude/rules/workflow.md`](rules/workflow.md) (ветки, коммиты).
> Общий контекст рабочего пространства из шести репозиториев — в `../CLAUDE.md`.

## Что это за репозиторий

NuGet-пакет типа **`Template`** (`dotnet new`), устанавливается как `dotnet new install Calabonga.Commandex.Shell.Develop.Template` и добавляет шаблон проекта **`wpfshell`** (identity `Calabonga.Commandex.Shell.Develop`). Из него разворачивается «Develop Shell» — урезанное WPF-приложение, эмулирующее настоящий `Calabonga.Commandex`, чтобы отлаживать **одну** команду «на месте» (по project reference), не собирая и не копируя DLL в полноценный Shell.

Тип в терминах версионирования рабочего пространства — **Framework**: `PackageVersion` пакета-шаблона в одном релизном цикле совпадает с версией `Engine` и остальных Framework-пакетов. Это content-пакет `dotnet new`: при `dotnet pack` ничего не восстанавливает, порядок публикации относительно `Engine`/`Processors` не важен (см. диаграмму в `../CLAUDE.md`). Версия `PackageReference` на `Calabonga.Commandex.Engine.Processors` **внутри** шаблонного `content/`-проекта резолвится только когда конечный пользователь выполнит `dotnet new wpfshell` и соберёт результат — поэтому она должна указывать на ту версию Processors/Engine, против которой собран целевой `Shell` (сейчас `4.0.0`, т.к. Samples-репозитории закреплены на мажоре 4).

## Структура

```
src/
  Calabonga.Commandex.Shell.Develop.Template.sln
  Calabonga.Commandex.Shell.Develop.Template/
    Calabonga.Commandex.Shell.Develop.Template.csproj   <- ПАКУЮЩИЙ проект (PackageType=Template, net10.0,
                                                            IncludeBuildOutput=false, ContentTargetFolders=content)
    content/                                             <- тело шаблона = готовое WPF-приложение
      .template.config/template.json                    <- манифест dotnet new (shortName wpfshell, sourceName …Shell.Develop)
      Calabonga.Commandex.Shell.Develop.csproj          <- net10.0-windows8.0, WinExe, UseWPF
      App.xaml / App.xaml.cs
      commandex.env                                     <- COMMANDS_FOLDER="<COMMANDS_FOLDER_NOT_PROVIDED>" (плейсхолдер)
      Engine/        DependencyContainer.cs  ServiceCollectionExtension.cs  SettingsFinder.cs  FocusExtension.cs
      ViewModels/    MainWindowsViewModel.cs  PreviewViewModel.cs  SettingsViewModel.cs
      Views/         MainWindow.xaml  PreviewView.xaml  SettingsView.xaml
      Zones/         IPreviewView.cs  IPreviewViewModel.cs
      Sample/        SampleCommand.cs  (+ SampleCommandDefinition)
```

`sourceName` в `template.json` = `Calabonga.Commandex.Shell.Develop` — при `dotnet new wpfshell -n My.Thing` эта строка заменяется во всех namespace, в имени `.csproj` и в `x:Class`.

## Сборка и публикация

```bash
dotnet build src/Calabonga.Commandex.Shell.Develop.Template.sln -c Release   # пакет (GeneratePackageOnBuild=True)
# отдельно собрать/запустить шаблонное приложение:
dotnet build src/Calabonga.Commandex.Shell.Develop.Template/content/Calabonga.Commandex.Shell.Develop.csproj
```

- **.NET 10 SDK**, только Windows. Пакующий проект — `net10.0`, шаблонное приложение — `net10.0-windows8.0`.
- Тестов в репозитории нет.
- **Публикация** — `.github/workflows/main.yml`: push в `main` → `dotnet pack` (не `build`!) → `dotnet nuget push *.nupkg --api-key $NUGET_API_KEY --source nuget.org --skip-duplicate`. Требуется secret `NUGET_API_KEY`.
- `NoDefaultExcludes=true` в пакующем `.csproj` (нужно, чтобы в пакет попала папка `.template.config`) — из-за него `bin`/`obj`/`.vs`/кэши **не** исключаются автоматически, их гасят вручную через `Exclude="content\**\bin\**;content\**\obj\**;content\**\.vs\**"` на `Content Include` и через явные `Remove` для `content\_ReSharper.Caches\**`.

## Как работает шаблонное приложение

1. **`App` ctor** — Serilog в `logs/local-.log` (посуточная ротация) → `SettingsFinder.Configure()` → `DependencyContainer.ConfigureServices()`.
2. **`SettingsFinder`** — `DotNetEnv.Env.Load("commandex.env", LoadOptions.TraversePath())`, читает `COMMANDS_FOLDER` (обязательный), `SETTINGS_FOLDER`, `SHOW_SEARCH_PANEL_ONSTARTUP`, `ARTIFACTS_FOLDER_NAME`, `NUGET_FEED_URL` → `AppSettings`. Ключей настоящего Shell `DEFAULT_VIEW_NAME` и OAuth-параметров здесь нет — в dev-shell нет списка команд и авторизации.
3. **`DependencyContainer`** — регистрирует движок Engine (`IDialogService`, `IZoneManager`, `IMvvmObjectFactory`, `ISettingsReaderConfiguration`, `IAppSettings` из `App.Current.Settings`), toast-`NotificationManager`, `AdvancedResultProcessor` + `Processor` из пакета Processors, `AddDialogComponent()` + `AddWizardComponent()`, затем `RegisterCommandsDefinitions()`. В конце — `ViewModelLocationProvider.SetDefaultViewModelFactory(...)`.
4. **`ServiceCollectionExtension.RegisterCommandsDefinitions`** — сканирует `AppDomain.CurrentDomain.BaseDirectory` на `*.dll`, `Assembly.LoadFrom`, ищет экспортированные типы `AppDefinition` + `ICommandexCommand`, вызывает `services.AddDefinitions(...)`. Т.е. команды подхватываются **из папки сборки**: достаточно добавить project reference на проект команды — его DLL окажется рядом. Сам dev-shell тоже сканируется, поэтому `SampleCommand` (скомпилированный внутрь) регистрируется всегда и приложение сразу рабочее.
5. **UI** — `MainWindow` содержит `ContentControl` с `zones:Zones.ZoneName="MainZone"` и `NotificationZone`. `MainWindowsViewModel` активирует зону `IPreviewView`/`IPreviewViewModel`. `PreviewViewModel` получает `IEnumerable<ICommandexCommand>` (все зарегистрированные), показывает ComboBox при >1, по кнопке `ExecuteAsync()` вызывает `SelectedCommand.ExecuteCommandAsync()` → `IResultProcessor.ProcessCommand(...)` → toast с результатом. Вкладка **Settings** через `SettingsViewModel` показывает содержимое `commandex.env`.

## Рабочий процесс разработчика команды

1. `dotnet new install Calabonga.Commandex.Shell.Develop.Template`
2. `dotnet new wpfshell -n Commandex.Developer.Shell` рядом с проектом команды.
3. Добавить в dev-shell project reference на WPF-class-library команды.
4. Прописать реальный путь в `commandex.env` (`COMMANDS_FOLDER`).
5. Собрать и запустить dev-shell — команда(ы) из reference-проекта авторегистрируются, выполняются с вкладки **Executor/Preview**. Логи — `logs/local-*.log`.

Ручная регистрация (`services.AddDefinitions(typeof(YourAppDefinition))` в `DependencyContainer.cs`) остаётся как запасной вариант, если авто-скан по каким-то причинам не подходит.

Result-процессор в `DependencyContainer` подключается вызовом `services.AddAdvancedResultProcessor()` из пакета Processors — той же строкой, что и в настоящем `Shell` (`IProcessor`/`IResultProcessor` регистрируются `Scoped`).

## Отличия от настоящего `Shell`

- Нет обнаружения команд по внешней папке (`CommandFinder` + `COMMANDS_FOLDER`-скан на произвольном каталоге), нет OAuth/identity, нет группировки/`CommandViewType`, нет `ArtifactService`/NuGet-зависимостей команд, нет глобальных перехватчиков исключений.
- Команды берутся только из `AppDomain.BaseDirectory` (project reference), рассчитано на 1–2 команды.
- `commandex.env` в шаблоне содержит плейсхолдер `COMMANDS_FOLDER="<COMMANDS_FOLDER_NOT_PROVIDED>"` — приложение упадёт с `ArgumentNullException`, пока путь не задан (ожидаемо, `SettingsFinder` бросает осознанно).
