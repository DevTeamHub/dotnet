using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevTeam.QueryMappings.Helpers;

/// <summary>
/// Class helper that should be used at the start of application. It reads all mappings from <see cref="IMappingsStorage"/> implementations 
/// and registers mappings for usage.
/// </summary>
/// <exception cref="MappingException">Thrown if we couldn't initialize mappings.</exception>
public static class MappingsConfiguration
{
    /// <summary>
    /// Search for implementations of <see cref="IMappingsStorage"/> inside of list of assemblies and registers mappings for usage.
    /// </summary>
    /// <param name="mappings">Singleton instance of <see cref="IMappingsList" /> that can be used to register mappings.</param>
    /// <param name="assemblies">List of assemblies where <see cref="IMappingsStorage"/> implementations are located.</param>
    /// <exception cref="MappingException">Thrown if we couldn't initialize mappings.</exception>
    public static void Register(IMappingsList mappings, params Assembly[] assemblies)
    {
        try
        {
            GetDefined(assemblies).ForEach(storage => Setup(storage, mappings));
        }
        catch (Exception exception)
        {
            throw new MappingException(Resources.GeneralInitializationException, exception);
        }
    }

    /// <summary>
    /// Search for list of <see cref="IMappingsStorage"/> implementations inside of assemblies.
    /// </summary>
    /// <param name="assemblies">List of assemblies where <see cref="IMappingsStorage"/> implementations are located.</param>
    /// <returns>List of <see cref="IMappingsStorage"/> implementations.</returns>
    private static List<IMappingsStorage> GetDefined(params Assembly[] assemblies)
    {
        var mappingsTypes = assemblies.SelectMany(x => x.GetTypes()).Where(t => typeof(IMappingsStorage).IsAssignableFrom(t));
        return mappingsTypes.Select(CreateStorageInstance).Cast<IMappingsStorage>().ToList();
    }

    /// <summary>
    /// Creates instance of <see cref="IMappingsStorage"/>. 
    /// </summary>
    /// <param name="storageType">Type that implements <see cref="IMappingsStorage"/> interface.</param>
    /// <returns>Instance of <see cref="IMappingsStorage"/> implementation.</returns>
    /// <exception cref="MappingException">Thrown if we couldn't initialize mappings.</exception>
    /// <exception cref="MissingMethodException">Thrown if <see cref="IMappingsStorage"/> implementation doesn't have any empty constructor.</exception>
    private static object CreateStorageInstance(Type storageType)
    {
        try
        {
            return Activator.CreateInstance(storageType);
        }
        catch (MissingMethodException mmmException)
        {
            var exceptionMessage = string.Format(Resources.NoEmptyConstructorInitializationException, storageType.FullName);
            throw new MappingException(exceptionMessage, mmmException);
        }
        catch (Exception exception)
        {
            throw new MappingException(Resources.GeneralMappingStorageException, exception);
        }
    }

    /// <summary>
    /// Registers mappings and saves them into memory for further usage.
    /// </summary>
    /// <param name="storage">Instance of <see cref="IMappingsStorage"/></param>
    /// <param name="mappings">Singleton instance of <see cref="IMappingsList" /> that can be used to register mappings.</param>
    /// <exception cref="MappingException">Thrown if error has happened during mappings initialization process.</exception>
    private static void Setup(IMappingsStorage storage, IMappingsList mappings)
    {
        try
        {
            storage.Setup(mappings);
        }
        catch (Exception exception)
        {
            var exceptionMessage = string.Format(Resources.MappingStorageSetupException, storage.GetType().FullName);
            throw new MappingException(exceptionMessage, exception);
        }
    }
}
