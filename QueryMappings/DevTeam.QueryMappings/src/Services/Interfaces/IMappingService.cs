using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Mappings;
using System.Collections.Generic;
using System.Linq;

namespace DevTeam.QueryMappings.Services.Interfaces;

/// <summary>
/// Extensions that allows easy to Apply mappings to <see cref="IQueryable"/> instances.
/// </summary>
public interface IMappingService
{
    /// <summary>
    /// Searches for <see cref="ExpressionMapping{TFrom, TTo}"/> in the Storage and applies mapping on <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    IQueryable<TModel> Map<TEntity, TModel>(IQueryable<TEntity> query, string? name = null);

    /// <summary>
    /// Searches for <see cref="ParameterizedMapping{TFrom, TTo, TArgs}"/> in the Storage and applies mapping on <see cref="IQueryable{T}"/>.
    /// Passes parameters into the mapping that can be used inside of the mapping expression.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if args are null or if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    IQueryable<TModel> Map<TEntity, TModel, TArgs>(IQueryable<TEntity> query, TArgs args, string? name = null);

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided model.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <param name="model">Instance of model to apply mapping to.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of destination object.</returns>
    /// <exception cref="MappingException">Thrown if args are null or if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    TTo Map<TFrom, TTo>(TFrom model, string? name = null);

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the every model in the provided list.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <param name="models">List of instances of the models to apply mapping to.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. List of instances of destination objects.</returns>
    /// <exception cref="MappingException">Thrown if args are null or if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    IEnumerable<TTo> Map<TFrom, TTo>(IEnumerable<TFrom> models, string? name = null);

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided model.
    /// Passes parameters into mapping that can be used inside of mapping expression.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="model">Instance of model to apply mapping to.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <returns>Result of mapping. Instance of destination object.</returns>
    /// <exception cref="MappingException">Thrown if args are null or if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    TTo Map<TFrom, TTo, TArgs>(TFrom model, TArgs args, string? name = null);

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the every model in the provided list.
    /// Passes parameters into the mapping that can be used inside of the mapping expression.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="models">List of instances of the models to apply mapping to.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <returns>Result of mapping. List of instances of destination objects.</returns>
    /// <exception cref="MappingException">Thrown if args are null or if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    IEnumerable<TTo> Map<TFrom, TTo, TArgs>(IEnumerable<TFrom> models, TArgs args, string? name = null);
}

/// <summary>
/// Extensions that allows easy to Apply mappings to <see cref="IQueryable"/> instances. 
/// It's a generic version of the service that also provides instance of a Database Context.
/// </summary>
/// <typeparam name="TContext">Type of EF Context that we want to inject inside of mapping.</typeparam>
public interface IMappingService<TContext> : IMappingService
{ }
