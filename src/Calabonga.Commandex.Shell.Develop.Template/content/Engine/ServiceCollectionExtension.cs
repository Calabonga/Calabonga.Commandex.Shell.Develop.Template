using Calabonga.Commandex.Engine.Base;
using Calabonga.Wpf.AppDefinitions;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.IO;
using System.Reflection;

namespace Calabonga.Commandex.Shell.Develop.Engine;

public static class ServiceCollectionExtension
{
    public static void RegisterCommandsDefinitions(this IServiceCollection services)
    {
        var commandBaseTypes = FindAllAbstractCommandTypes().ToList();

        var modulesDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        var files = modulesDirectory.GetFiles("*.dll");
        if (!files.Any())
        {
            Log.Information("No modules were found in folder");
            return;
        }

        var typesFound = new List<Type>();
        foreach (var fileInfo in files)
        {
            try
            {
                var assembly = Assembly.LoadFrom(fileInfo.FullName) ?? throw new ArgumentNullException();
                var exportedTypes = assembly.GetExportedTypes();
                var modulesTypes = exportedTypes.Where(AppDefinitionFindPredicate).ToList();
                if (!modulesTypes.Any())
                {
                    continue;
                }

                var commands = exportedTypes.Where(CommandexPredicate).ToList();
                if (!commands.Any())
                {
                    var error = new AppDefinitionsNotFoundException($"AppDefinition found in {fileInfo.FullName}, but there are no ICommandexCommand implementation were found");
                    Log.Logger.Error(error, error.Message);
                    continue;
                }

                foreach (var commandType in commands)
                {
                    var typeName = "";
                    foreach (var type in commandBaseTypes.Where(type => IsAssignableToGenericType(commandType, type)))
                    {
                        typeName = GetNameWithoutGenericArity(type);
                        break;
                    }

                    Log.Logger.Debug("[{Command}] is type of {Type}", commandType.Name, typeName);
                }

                typesFound.AddRange(modulesTypes);

            }
            catch (Exception exception)
            {
                Log.Logger.Error(exception, exception.Message);
            }
        }

        if (typesFound.Any())
        {
            services.AddDefinitions(typesFound.ToArray());
        }
    }

    private static bool AppDefinitionFindPredicate(Type type)
    {
        return type is { IsAbstract: false, IsInterface: false } && typeof(AppDefinition).IsAssignableFrom(type);
    }

    private static bool CommandexPredicate(Type type)
    {
        return type is { IsAbstract: false, IsInterface: false } && typeof(ICommandexCommand).IsAssignableFrom(type);
    }

    private static IEnumerable<Type> FindAllAbstractCommandTypes()
    {
        var typeCommand = typeof(ICommandexCommand);

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(x => typeCommand.IsAssignableFrom(x) && x is { IsAbstract: true, IsClass: true });

        foreach (var type in types)
        {
            //do stuff
            yield return type;
        }
    }

    private static bool IsAssignableToGenericType(Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == genericType))
        {
            return true;
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        var baseType = givenType.BaseType;
        return baseType != null && IsAssignableToGenericType(baseType, genericType);
    }

    private static string GetNameWithoutGenericArity(this Type t)
    {
        var name = t.Name;
        var index = name.IndexOf('`');
        return index == -1 ? name : name[..index];
    }
}
