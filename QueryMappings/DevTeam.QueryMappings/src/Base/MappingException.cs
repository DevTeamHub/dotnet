using DevTeam.QueryMappings.Properties;
using System;
using System.Runtime.Serialization;

namespace DevTeam.QueryMappings.Base;

/// <summary>
/// Special exception that is used to describe every exception that happens inside of QueryMappings library.
/// Implements standart Exception pattern.
/// </summary>
[Serializable]
public class MappingException : ApplicationException
{
    /// <summary>
    /// Creates instance of MappingException
    /// </summary>
    public MappingException()
    { }

    /// <summary>
    /// Creates instance of MappingException
    /// </summary>
    /// <param name="message">Custom exception message.</param>
    public MappingException(string message)
        : base(message)
    { }

    /// <summary>
    /// Creates instance of MappingException
    /// </summary>
    /// <param name="message">Custom exception message.</param>
    /// <param name="inner">Exception that will be presented as Inner Exception.</param>
    public MappingException(string message, Exception inner)
        : base(message, inner)
    { }

    /// <summary>
    /// Creates instance of MappingException
    /// </summary>
    /// <param name="info">Serialization Info</param>
    /// <param name="context">Streaming context</param>
    protected MappingException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    { }

    /// <summary>
    /// Helps to build descriptive message if we are using incorrect version of Map method.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <param name="requestedType">Type of mapping that was found in storage instead of expected type.</param>
    /// <param name="actualType">Expected type of mapping that we tried to find.</param>
    /// <param name="innerException">Exception that has happened during incorrect conversion of mapping.</param>
    /// <returns>Descriptive exception with detailed information about exception.</returns>
    public static MappingException HandleMappingError<TFrom, TTo>(MappingType requestedType, MappingType actualType, Exception innerException)
    {
        var exceptionMessage = string.Format(Resources.InvalidCastMappingException, typeof(TFrom).Name, typeof(TTo).Name);

        if (requestedType.HasFlag(MappingType.Parameterized) && !actualType.HasFlag(MappingType.Parameterized))
        {
            exceptionMessage += Resources.ArgumentsAreNotNeeded;
        }

        if (!requestedType.HasFlag(MappingType.Parameterized) && actualType.HasFlag(MappingType.Parameterized))
        {
            exceptionMessage += Resources.ArgumentsHaventBeenPassed;
        }

        if (!requestedType.HasFlag(MappingType.Query) && actualType.HasFlag(MappingType.Query))
        {
            exceptionMessage += Resources.ContextHaventBeenInjected;
        }

        return new MappingException(exceptionMessage, innerException);
    }
}
