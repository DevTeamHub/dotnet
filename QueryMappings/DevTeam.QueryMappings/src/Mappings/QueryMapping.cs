using DevTeam.QueryMappings.Base;
using System;
using System.Linq;

namespace DevTeam.QueryMappings.Mappings;

/// <summary>
/// Describes mapping from one type to another using provided expression and Entity Framework Context that is injected inside of the expression. 
/// </summary>
/// <typeparam name="TFrom">Source type of mapping.</typeparam>
/// <typeparam name="TTo">Destination type of mapping.</typeparam>
/// <typeparam name="TContext">Entity Framework Context type.</typeparam>
public class QueryMapping<TFrom, TTo, TContext> : Mapping<TTo>
{
    /// <summary>
    /// Creates instance of <see cref="QueryMapping{TFrom, TTo, TContext}"/> class.
    /// </summary>
    /// <param name="mapping">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance. Input parameters contain EF Context that can be used inside of mapping.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    public QueryMapping(Func<IQueryable<TFrom>, TContext, IQueryable<TTo>> mapping, string? name = null)
        : base(typeof(TFrom), typeof(TTo), null, typeof(TContext), MappingType.Query, name)
    {
        _mapping = mapping;
    }

    private readonly Func<IQueryable<TFrom>, TContext, IQueryable<TTo>> _mapping;

    /// <summary>
    /// Applies expression on <see cref="IQueryable{T}"/> instance.
    /// EF Context will be injected inside of the expression.
    /// </summary>
    /// <param name="query"><see cref="IQueryable{T}"/> instance.</param>
    /// <param name="context">Injected EF Context.</param>
    /// <returns>New <see cref="IQueryable{T}"/> instance with applied expression.</returns>
    public IQueryable<TTo> Apply(IQueryable<TFrom> query, TContext context)
    {
        return _mapping.Invoke(query, context);
    }
}
