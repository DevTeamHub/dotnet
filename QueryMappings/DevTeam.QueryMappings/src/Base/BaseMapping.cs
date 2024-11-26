using DevTeam.QueryMappings.Properties;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevTeam.QueryMappings.Base;

/// <summary>
/// Describes one of 4 types of mappings: Expression Mapping, Parameterized Mapping, Query Mapping or Parameterized Query Mapping.
/// Parameterized Query Mapping is just a combination of Parameterized and Query Mappings, so it's described with Flag enum.
/// </summary>
[Flags]
public enum MappingType : byte
{
    /// <summary>
    /// Expression Mapping: <see cref="Mappings.ExpressionMapping{TFrom, TTo}"/>
    /// </summary>
    Expression = 1,

    /// <summary>
    /// Parameterized Mapping: <see cref="Mappings.ParameterizedMapping{TFrom, TTo, TArgs}"/>
    /// </summary>
    Parameterized = 2,

    /// <summary>
    /// Query Mapping: <see cref="Mappings.QueryMapping{TFrom, TTo, TContext}"/>
    /// </summary>
    Query = 4,

    /// <summary>
    /// Parameterized Query Mapping: <see cref="Mappings.ParameterizedQueryMapping{TFrom, TTo, TArgs, TContext}"/>
    /// </summary>
    ParemeterizedQuery = 6
}

/// <summary>
/// Base class of every mapping. Contains information about direction of mapping (From -> To), mapping type and mapping name (for Named Mappings).
/// </summary>
public abstract class Mapping
{
    /// <summary>
    /// Source type of mapping.
    /// </summary>
    public Type From { get; protected set; }

    /// <summary>
    /// Destination type of mapping.
    /// </summary>
    public Type To { get; protected set; }

    /// <summary>
    /// Returns type of required arguments to perform the mapping.
    /// </summary>
    public Type? ArgumentsType { get; protected set; }

    /// <summary>
    /// Returns type of required Database Context to perform the mapping.
    /// </summary>
    public Type? ContextType { get; protected set; }

    /// <summary>
    /// Type of mapping. Described with help of <see cref="MappingType"/> enum.
    /// </summary>
    public MappingType MappingType { get; protected set; }

    /// <summary>
    /// Name of mapping. Can be used when it's needed to have more than one mapping from type A to type B.
    /// Name will be identifier of mapping in this case.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Constructor of mapping class. Allows to set up direction of mapping.
    /// </summary>
    /// <param name="from">Source type of mapping.</param>
    /// <param name="to">Destination type of mapping.</param>
    /// <param name="argumentsType">Type of Arguments of they passed into the mapping.</param>
    /// <param name="contextType">Type of Database Context if it's required by the mapping.</param>
    /// <param name="mappingType">Type of mapping. Described with help of <see cref="MappingType"/> enum.</param>
    /// <param name="name">Name of mapping. Used as identifier for mappings with the same direction (From -> To).</param>
    protected Mapping(Type from, Type to, Type? argumentsType, Type? contextType, MappingType mappingType, string? name = null)
    {
        From = from;
        To = to;
        ArgumentsType = argumentsType;
        ContextType = contextType;
        Name = name;
        MappingType = mappingType;
    }

    /// <summary>
    /// Checks if current mapping has described direction.
    /// </summary>
    /// <typeparam name="TFirst">Source type of mapping.</typeparam>
    /// <typeparam name="TSecond">Destination type of mapping.</typeparam>
    /// <param name="name">Name of mapping. Used as identifier for mappings with the same direction (From -> To).</param>
    /// <returns>Result of comparison that signalizes if current mapping has the same direction.</returns>
    public bool Is<TFirst, TSecond>(string? name = null)
    {
        return Is(typeof(TFirst), typeof(TSecond), name);
    }

    /// <summary>
    /// Checks if current mapping has described direction.
    /// </summary>
    /// <param name="from">Source type of mapping.</param>
    /// <param name="to">Destination type of mapping.</param>
    /// <param name="name">Name of mapping. Used as identifier for mappings with the same direction (From -> To).</param>
    /// <returns>Result of comparison that signalizes if current mapping has the same direction.</returns>
    public bool Is(Type from, Type to, string? name = null)
    {
        if (name != null && Name != name)
            return false;

        return from == From && to == To;
    }

    /// <summary>
    /// Validate if provided by user arguments are the same type as arguments that mapping requires.
    /// </summary>
    /// <exception cref="MappingException">Thrown if type of provided arguments is different from the one used in the mapping.</exception>
    public void ValidateArguments<TArgs>()
    {
        if (ArgumentsType != null && ArgumentsType != typeof(TArgs))
        {
            throw new MappingException(string.Format(Resources.ArgumentsOfIncorrectType, From.Name, To.Name, ArgumentsType, typeof(TArgs)));
        }
    }

    /// <summary>
    /// Validate if provided by user Database Context are the same type as Database Context that mapping requires.
    /// </summary>
    /// <exception cref="MappingException">Thrown if type of provided Context type is different from the one used in the mapping.</exception>
    public void ValidateContext<TContext>()
    {
        if (ContextType != null && ContextType != typeof(TContext))
        {
            throw new MappingException(string.Format(Resources.ContextOfIncorrectType, From.Name, To.Name, ContextType, typeof(TContext)));
        }
    }
}

/// <summary>
/// Base class of every mapping. Contains information about direction of mapping (From -> To), mapping type and mapping name (for Named Mappings).
/// Also stores the information about the result of the mapping and can perform different operations on the result. 
/// </summary>
public abstract class Mapping<TResult> : Mapping
{
    private Func<TResult, TResult>? _thenFunc;

    /// <summary>
    /// Constructor of mapping class. Allows to set up direction of mapping.
    /// </summary>
    /// <param name="from">Source type of mapping.</param>
    /// <param name="to">Destination type of mapping.</param>
    /// <param name="argumentsType">Type of Arguments of they passed into the mapping.</param>
    /// <param name="contextType">Type of Database Context if it's required by the mapping.</param>
    /// <param name="mappingType">Type of mapping. Described with help of <see cref="MappingType"/> enum.</param>
    /// <param name="name">Name of mapping. Used as identifier for mappings with the same direction (From -> To).</param>
    protected Mapping(Type from, Type to, Type? argumentsType, Type? contextType, MappingType mappingType, string? name = null)
        : base(from, to, argumentsType, contextType, mappingType, name)
    { }

    /// <summary>
    /// Adds second mapping function that will be executed after a request to the database completed. 
    /// It can be helpful in case if we need to make some additional model transformations that don't require any complex business logic.
    /// Use it for performance optimizations to avoid unnecessary transformations during the request to the database.
    /// </summary>
    /// <param name="thenFunc">Func expression that will be applied to a result of the database request.</param>
    public void Then(Func<TResult, TResult> thenFunc)
    {
        _thenFunc = thenFunc;
    }

    /// <summary>
    /// Applies Then function to every element of the <see cref="IEnumerable{T}"/> instance.
    /// </summary>
    /// <param name="list"><see cref="IEnumerable{T}"/> instance.</param>
    /// <returns>New <see cref="IEnumerable{T}"/> instance as a result of applied Then function.</returns>
    public IEnumerable<TResult> ApplyThen(IEnumerable<TResult> list)
    {
        return _thenFunc != null ? list.Select(_thenFunc) : list;
    }

    /// <summary>
    /// Applies Then function to the provided model.
    /// </summary>
    /// <param name="model">Model apply Then function to.</param>
    /// <returns>The result model after Then function is applied.</returns>
    public TResult ApplyThen(TResult model)
    {
        return _thenFunc != null ? _thenFunc(model) : model;
    }
}
