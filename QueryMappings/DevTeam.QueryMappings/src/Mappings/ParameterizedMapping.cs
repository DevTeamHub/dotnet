using DevTeam.QueryMappings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DevTeam.QueryMappings.Mappings;

/// <summary>
/// Describes mapping from one type to another using provided expression and arguments that can be used inside of the expression. 
/// </summary>
/// <typeparam name="TFrom">Source type of mapping.</typeparam>
/// <typeparam name="TTo">Destination type of mapping.</typeparam>
/// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
public class ParameterizedMapping<TFrom, TTo, TArgs> : Mapping<TTo>
{
    /// <summary>
    /// Creates instance of <see cref="ParameterizedMapping{TFrom, TTo, TArgs}"/> class.
    /// </summary>
    /// <param name="mapping">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance. Input parameter is an object of arguments that will be used inside of expression.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    public ParameterizedMapping(Func<TArgs, Expression<Func<TFrom, TTo>>> mapping, string? name = null)
        : base(typeof(TFrom), typeof(TTo), typeof(TArgs), null, MappingType.Parameterized, name)
    {
        _mapping = mapping;
    }

    private readonly Func<TArgs, Expression<Func<TFrom, TTo>>> _mapping;

    /// <summary>
    /// Applies expression to <see cref="IQueryable{T}"/> instance.
    /// Arguments can be used inside of the expression.
    /// </summary>
    /// <param name="query"><see cref="IQueryable{T}"/> instance.</param>
    /// <param name="args">Arguments that we pass into mapping expression.</param>
    /// <returns>New <see cref="IQueryable{T}"/> instance with applied expression.</returns>
    public IQueryable<TTo> Apply(IQueryable<TFrom> query, TArgs args)
    {
        var expression = _mapping.Invoke(args);
        return query.Select(expression);
    }

    /// <summary>
    /// Applies expression to <see cref="IEnumerable{T}"/> instance.
    /// Arguments can be used inside of the expression.
    /// </summary>
    /// <param name="query"><see cref="IEnumerable{T}"/> instance.</param>
    /// <param name="args">Arguments that we pass into mapping expression.</param>
    /// <returns>New <see cref="IEnumerable{T}"/> instance with applied expression.</returns>
    public IEnumerable<TTo> Apply(IEnumerable<TFrom> query, TArgs args)
    {
        var expression = _mapping.Invoke(args);
        var func = expression.Compile();
        return query.Select(func);
    }

    /// <summary>
    /// Applies expression to source model instance.
    /// Arguments can be used inside of the expression.
    /// </summary>
    /// <param name="model">Source model instance.</param>
    /// <param name="args">Arguments that we pass into mapping expression.</param>
    /// <returns>New destination model instance.</returns>
    public TTo Apply(TFrom model, TArgs args)
    {
        var expression = _mapping.Invoke(args);
        var func = expression.Compile();
        return func(model);
    }
}
