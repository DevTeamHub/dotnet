using DevTeam.QueryMappings.Base;
using System;
using System.Linq;

namespace DevTeam.QueryMappings.Mappings;

/// <summary>
/// Describes mapping from one type to another using provided expression, arguments that can be used inside of the expression and Entity Framework Context that is injected inside of the expression. 
/// </summary>
/// <typeparam name="TFrom">Source type of mapping.</typeparam>
/// <typeparam name="TTo">Destination type of mapping.</typeparam>
/// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
/// <typeparam name="TContext">Entity Framework Context type.</typeparam>
public class ParameterizedQueryMapping<TFrom, TTo, TArgs, TContext> : Mapping<TTo>
{
    /// <summary>
    /// Creates instance of <see cref="ParameterizedQueryMapping{TFrom, TTo, TArgs, TContext}"/> class.
    /// </summary>
    /// <param name="mapping">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance. Input parameters contain arguments and EF Context that can be used inside of mapping.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    public ParameterizedQueryMapping(Func<TArgs, Func<IQueryable<TFrom>, TContext, IQueryable<TTo>>> mapping, string? name = null)
        : base(typeof(TFrom), typeof(TTo), typeof(TArgs), typeof(TContext), MappingType.ParemeterizedQuery, name)
    {
        _mapping = mapping;
    }

    private readonly Func<TArgs, Func<IQueryable<TFrom>, TContext, IQueryable<TTo>>> _mapping;

    /// <summary>
    /// Applies expression on <see cref="IQueryable{T}"/> instance.
    /// Arguments can be used inside of the expression.
    /// EF Context will be injected inside of the expression.
    /// </summary>
    /// <param name="query"><see cref="IQueryable{T}"/> instance.</param>
    /// <param name="args">Arguments that we pass into mapping expression.</param>
    /// <param name="context">Injected EF Context.</param>
    /// <returns>New <see cref="IQueryable{T}"/> instance with applied expression.</returns>
    public IQueryable<TTo> Apply(IQueryable<TFrom> query, TArgs args, TContext context)
    {
        var expression = _mapping.Invoke(args);
        return expression.Invoke(query, context);
    }
}
