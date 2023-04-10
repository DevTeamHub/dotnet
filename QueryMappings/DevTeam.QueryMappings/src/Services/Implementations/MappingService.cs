using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;
using DevTeam.QueryMappings.Mappings;
using DevTeam.QueryMappings.Properties;
using DevTeam.QueryMappings.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DevTeam.QueryMappings.Services.Implementations;

/// <summary>
/// Extensions that allows easy to Apply mappings to <see cref="IQueryable"/> instances.
/// </summary>
public class MappingService : IMappingService
{
    private readonly IMappingsList _mappingsList;

    /// <summary>
    /// Constructor, depends on <see cref="IMappingsList" />
    /// </summary>
    public MappingService(IMappingsList mappingsList)
    {
        _mappingsList = mappingsList;
    }

    #region Expression Query Mapping

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    public virtual IQueryable<TModel> Map<TEntity, TModel>(IQueryable<TEntity> query, string? name = null)
    {
        return Map<TEntity, TModel>(query, MappingType.Expression, name);
    }

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="mappingType">Type of mapping that we will be searching for.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    protected virtual IQueryable<TModel> Map<TEntity, TModel>(IQueryable<TEntity> query, MappingType mappingType, string? name = null)
    {
        return ApplyMapping<TEntity, TModel, IQueryable<TModel>>(
            mapping => Map<TEntity, TModel>(query, mapping),
            mappingType,
            name
        );
    }

    /// <summary>
    /// Applies the mapping to the provided <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="mapping"><see cref="ExpressionMapping{TFrom, TTo}"/> mapping.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    protected virtual IQueryable<TModel> Map<TEntity, TModel>(IQueryable<TEntity> query, Mapping mapping)
    {
        var expressionMapping = (ExpressionMapping<TEntity, TModel>)mapping;
        return expressionMapping.Apply(query);
    }

    #endregion

    #region Expression Object Mapping

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided model.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <param name="model">Instance of model to apply mapping to.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of destination object.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    public virtual TTo Map<TFrom, TTo>(TFrom model, string? name = null)
    {
        return Map<TFrom, TTo>(model, MappingType.Expression, name);
    }

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided model.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <param name="model">Instance of model to apply mapping to.</param>
    /// <param name="mappingType">Type of mapping that we will be searching for.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of destination object.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    protected virtual TTo Map<TFrom, TTo>(TFrom model, MappingType mappingType, string? name = null)
    {
        return ApplyMapping<TFrom, TTo, TTo>(
            mapping => Map<TFrom, TTo>(model, mapping),
            mappingType,
            name
        );
    }

    /// <summary>
    /// Applies the mapping to the provided <see cref="IQueryable{T}"/>.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <param name="model">Instance of model to apply mapping to.</param>
    /// <param name="mapping"><see cref="ExpressionMapping{TFrom, TTo}"/> mapping.</param>
    /// <returns>Result of mapping. Instance of destination object.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    protected virtual TTo Map<TFrom, TTo>(TFrom model, Mapping mapping)
    {
        var expressionMapping = (ExpressionMapping<TFrom, TTo>)mapping;
        return expressionMapping.Apply(model);
    }

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the every model in the provided list.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <param name="models">List of instances of the models to apply mapping to.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. List of instances of destination objects.</returns>
    /// <exception cref="MappingException">Thrown if args are null or if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    public virtual List<TTo> Map<TFrom, TTo>(List<TFrom> models, string? name = null)
    {
        return models.Select(item => Map<TFrom, TTo>(item, name)).ToList();
    }

    #endregion

    #region Parameterized Query Mapping

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided <see cref="IQueryable{T}"/>.
    /// Passes parameters into mapping that can be used inside of mapping expression.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if args are null or if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    public virtual IQueryable<TModel> Map<TEntity, TModel, TArgs>(IQueryable<TEntity> query, TArgs args, string? name = null)
    {
        return Map<TEntity, TModel, TArgs>(query, MappingType.Parameterized, args, name);
    }

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided <see cref="IQueryable{T}"/>.
    /// Passes parameters into mapping that can be used inside of mapping expression.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="mappingType">Type of mapping that we will be searching for.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if args are null or if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    protected IQueryable<TModel> Map<TEntity, TModel, TArgs>(IQueryable<TEntity> query, MappingType mappingType, TArgs args, string? name = null)
    {
        if (args == null)
            throw new MappingException(Resources.ArgumentsAreRequiredException);

        return ApplyMapping<TEntity, TModel, IQueryable<TModel>>(
            mapping => Map<TEntity, TModel, TArgs>(query, args, mapping),
            mappingType,
            name
        );
    }

    /// <summary>
    /// Applies the mapping to the provided <see cref="IQueryable{T}"/>.
    /// Passes parameters into mapping that can be used inside of mapping expression.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Arguments type used in the mapping.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <param name="mapping"><see cref="ExpressionMapping{TFrom, TTo}"/> mapping.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map method or if mapping wasn't found.</exception>
    protected virtual IQueryable<TModel> Map<TEntity, TModel, TArgs>(IQueryable<TEntity> query, TArgs args, Mapping mapping)
    {
        mapping.ValidateArguments<TArgs>();
        var parameterizedMapping = (ParameterizedMapping<TEntity, TModel, TArgs>)mapping;
        return parameterizedMapping.Apply(query, args);
    }

    #endregion

    #region Parameterized Object Mapping

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided model.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="model">Instance of model to apply mapping to.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of destination object.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    public virtual TTo Map<TFrom, TTo, TArgs>(TFrom model, TArgs args, string? name = null)
    {
        return Map<TFrom, TTo, TArgs>(model, MappingType.Parameterized, args, name);
    }

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided model.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="model">Instance of model to apply mapping to.</param>
    /// <param name="mappingType">Type of mapping that we will be searching for.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of destination object.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    protected virtual TTo Map<TFrom, TTo, TArgs>(TFrom model, MappingType mappingType, TArgs args, string? name = null)
    {
        if (args == null)
            throw new MappingException(Resources.ArgumentsAreRequiredException);

        return ApplyMapping<TFrom, TTo, TTo>(
            mapping => Map<TFrom, TTo, TArgs>(model, args, mapping),
            mappingType,
            name
        );
    }

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided model.
    /// Passes parameters into mapping that can be used inside of mapping expression.
    /// </summary>
    /// <typeparam name="TFrom">Source model for mapping.</typeparam>
    /// <typeparam name="TTo">Destination model for mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="model">Instance of model to apply mapping to.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <param name="mapping"><see cref="ExpressionMapping{TFrom, TTo}"/> mapping.</param>
    /// <returns>Result of mapping. Instance of destination object.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map() method or if mapping wasn't found.</exception>
    protected virtual TTo Map<TFrom, TTo, TArgs>(TFrom model, TArgs args, Mapping mapping)
    {
        mapping.ValidateArguments<TArgs>();
        var parameterizedMapping = (ParameterizedMapping<TFrom, TTo, TArgs>)mapping;
        return parameterizedMapping.Apply(model, args);
    }

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
    public virtual List<TTo> Map<TFrom, TTo, TArgs>(List<TFrom> models, TArgs args, string? name = null)
    {
        return models.Select(item => Map<TFrom, TTo, TArgs>(item, args, name)).ToList();
    }

    #endregion

    /// <summary>
    /// Wrapper that helps handle exceptions for every Map overload.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <typeparam name="TResult">Instance of the result of the mapping.</typeparam>
    /// <param name="applyFunction">Map function with main logic.</param>
    /// <param name="mappingType">Type of mapping to search for.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Instance of the result of the mapping.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map method or if mapping wasn't found.</exception>
    protected internal TResult ApplyMapping<TEntity, TModel, TResult>(Func<Mapping, TResult> applyFunction, MappingType mappingType, string? name = null)
    {
        var mapping = _mappingsList.Get<TEntity, TModel>(name);

        try
        {
            return applyFunction(mapping);
        }
        catch (MappingException)
        {
            throw;
        }
        catch (InvalidCastException castException)
        {
            var exception = MappingException.HandleMappingError<TEntity, TModel>(mappingType, mapping.MappingType, castException);
            throw exception;
        }
        catch (Exception exception)
        {
            throw new MappingException(Resources.ApplyMappingException, exception);
        }
    }
}

/// <summary>
/// Extension of the <see cref="MappingService" /> that allows to provide Database Context instances into the mappings. 
/// </summary>
/// <typeparam name="TContext">Type of the Database Context that we want to inject inside of the mapping.</typeparam>
public class MappingService<TContext> : MappingService, IMappingService<TContext>
{
    private readonly TContext _context;

    /// <summary>
    /// Constructor, depends on <see cref="IMappingsList" />
    /// </summary>
    public MappingService(TContext context, IMappingsList mappingsList)
        : base(mappingsList)
    {
        _context = context;
    }

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided <see cref="IQueryable{T}"/>.
    /// Injects Database Context of TContext type into mapping if it depends on the Context.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map method or if mapping wasn't found.</exception>
    public override IQueryable<TModel> Map<TEntity, TModel>(IQueryable<TEntity> query, string? name = null)
    {
        return Map<TEntity, TModel>(query, MappingType.Query, name);
    }

    /// <summary>
    /// Applies the mapping to the provided <see cref="IQueryable{T}"/>. If the mapping requires a Context as a dependency, it will also provides 
    /// the context as last argument. 
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="mapping"><see cref="ExpressionMapping{TFrom, TTo}"/> mapping.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map method or if mapping wasn't found.</exception>
    protected override IQueryable<TModel> Map<TEntity, TModel>(IQueryable<TEntity> query, Mapping mapping)
    {
        if (mapping.MappingType.HasFlag(MappingType.Query))
        {
            mapping.ValidateContext<TContext>();
            var queryMapping = (QueryMapping<TEntity, TModel, TContext>)mapping;
            return queryMapping.Apply(query, _context);
        }

        return base.Map<TEntity, TModel>(query, mapping);
    }

    /// <summary>
    /// Searches for the mapping in the Storage and applies mapping to the provided <see cref="IQueryable{T}"/>.
    /// Passes parameters into mapping that can be used inside of mapping expression.
    /// Injects Database Context of TContext type into mapping if it depends on the Context.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if args are null or if we are using incorrect version of Map method or if mapping wasn't found.</exception>
    public override IQueryable<TModel> Map<TEntity, TModel, TArgs>(IQueryable<TEntity> query, TArgs args, string? name = null)
    {
        return Map<TEntity, TModel, TArgs>(query, MappingType.ParemeterizedQuery, args, name);
    }

    /// <summary>
    /// Applies the mapping to the provided <see cref="IQueryable{T}"/>. If the mapping requires a Context as a dependency, it will also provides 
    /// the context as last argument. 
    /// Passes parameters into mapping that can be used inside of mapping expression.
    /// </summary>
    /// <typeparam name="TEntity">Source type of mapping.</typeparam>
    /// <typeparam name="TModel">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Arguments type used in the mapping.</typeparam>
    /// <param name="query">Instance of <see cref="IQueryable{T}"/> to apply mapping to.</param>
    /// <param name="args">Arguments that we want to pass into mapping to use them inside of mapping expression.</param>
    /// <param name="mapping"><see cref="ExpressionMapping{TFrom, TTo}"/> mapping.</param>
    /// <returns>Result of mapping. Instance of <see cref="IQueryable{T}"/> object with applied mapping.</returns>
    /// <exception cref="MappingException">Thrown if we are using incorrect version of Map method or if mapping wasn't found.</exception>
    protected override IQueryable<TModel> Map<TEntity, TModel, TArgs>(IQueryable<TEntity> query, TArgs args, Mapping mapping)
    {
        if (mapping.MappingType == MappingType.ParemeterizedQuery)
        {
            mapping.ValidateArguments<TArgs>();
            mapping.ValidateContext<TContext>();
            var parameterizedMapping = (ParameterizedQueryMapping<TEntity, TModel, TArgs, TContext>)mapping;
            return parameterizedMapping.Apply(query, args, _context);
        }

        return base.Map<TEntity, TModel, TArgs>(query, args, mapping);
    }
}
