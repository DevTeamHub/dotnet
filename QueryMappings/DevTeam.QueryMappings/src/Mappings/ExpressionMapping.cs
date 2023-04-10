using DevTeam.QueryMappings.Base;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DevTeam.QueryMappings.Mappings;

/// <summary>
/// Describes simple mapping from one type to another using provided expression. 
/// </summary>
/// <typeparam name="TFrom">Source type of mapping.</typeparam>
/// <typeparam name="TTo">Destination type of mapping.</typeparam>
public class ExpressionMapping<TFrom, TTo> : Mapping<TTo>
{
    /// <summary>
    /// Creates instance of <see cref="ExpressionMapping{TFrom, TTo}"/> class.
    /// </summary>
    /// <param name="mapping">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    public ExpressionMapping(Expression<Func<TFrom, TTo>> mapping, string? name = null)
        : base(typeof(TFrom), typeof(TTo), null, null, MappingType.Expression, name)
    {
        _mapping = mapping;
        _mappingFunc = mapping.Compile();
    }

    private readonly Expression<Func<TFrom, TTo>> _mapping;
    private readonly Func<TFrom, TTo> _mappingFunc;

    /// <summary>
    /// Applies simple expression to <see cref="IQueryable{T}"/> instance.
    /// </summary>
    /// <param name="query"><see cref="IQueryable{T}"/> instance.</param>
    /// <returns>New <see cref="IQueryable{T}"/> instance with applied expression.</returns>
    public IQueryable<TTo> Apply(IQueryable<TFrom> query)
    {
        return query.Select(_mapping);
    }

    /// <summary>
    /// Applies simple expression to the provided model.
    /// </summary>
    /// <param name="model">Source model instance.</param>
    /// <returns>Destination model instance.</returns>
    public TTo Apply(TFrom model)
    {
        return _mappingFunc(model);
    }
}
