using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Mappings;
using DevTeam.QueryMappings.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DevTeam.QueryMappings.Helpers;

/// <summary>
/// Internal In-Memory mappings storage. Holds all registered mapping.
/// </summary>
public class MappingsList : IMappingsList
{
    private readonly List<Mapping> Mappings;

    /// <summary>
    /// Default constructor
    /// </summary>
    public MappingsList()
    {
        Mappings = new List<Mapping>();
    }

    /// <summary>
    /// Check for the presence of a mapping with described direction.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <param name="name">Name of the mapping, if we want to check for the presence of a mapping registered with some specific name. Should be null if we want to check mapping without name.</param>
    /// <returns>Result of check for the presence of a mapping.</returns>
    /// <exception cref="MappingException">Thrown if more than one mapping found and wasn't enough information to resolve which exactly is correct one.</exception>
    public bool Exist<TFrom, TTo>(string? name = null)
    {
        return GetMapping<TFrom, TTo>(name) != null;
    }

    /// <summary>
    /// Searches for a mapping with described direction.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Instance of mapping with described direction.</returns>
    /// <exception cref="MappingException">Thrown if mapping wasn't found or if more than one mapping found and wasn't enough information to resolve which exactly is correct one.</exception>
    public Mapping Get<TFrom, TTo>(string? name = null)
    {
        var mapping = GetMapping<TFrom, TTo>(name);

        if (mapping == null)
        {
            var exceptionMessage = string.Format(Resources.MappingNotFoundException, typeof(TFrom).Name, typeof(TTo).Name);
            throw new MappingException(exceptionMessage);
        }

        return mapping;
    }

    /// <summary>
    /// Get mapping with described direction.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <param name="name">Name of the mapping, if we want to get mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <returns>Instance of mapping with described direction. Null if mapping not found</returns>
    /// <exception cref="MappingException">Thrown if more than one mapping found and wasn't enough information to resolve which exactly is correct one.</exception>
    private Mapping? GetMapping<TFrom, TTo>(string? name = null)
    {
        try
        {
            return Mappings.SingleOrDefault(m => m.Is<TFrom, TTo>(name));
        }
        catch (InvalidOperationException ioeException)
        {
            var exceptionMessage = string.Empty;
            var namedMappings = Mappings.Where(m => m.Is<TFrom, TTo>() && !string.IsNullOrEmpty(m.Name)).ToList();

            if (namedMappings.Count > 0 && string.IsNullOrEmpty(name))
            {
                exceptionMessage = string.Format(Resources.NameIsNullWhenSearchForNamedMappingException, typeof(TFrom).Name, typeof(TTo).Name);
            }
            else if (namedMappings.Count == 0)
            {
                exceptionMessage = string.Format(Resources.MoreThanOneMappingFoundException, typeof(TFrom).Name, typeof(TTo).Name);
            }

            throw new MappingException(exceptionMessage, ioeException);
        }
    }

    /// <summary>
    /// Adds mapping into Storage.
    /// </summary>
    /// <param name="mapping">Mapping to add into storage.</param>
    private void Add(Mapping mapping)
    {
        Mappings.Add(mapping);
    }

    /// <summary>
    /// Adds <see cref="ExpressionMapping{TFrom, TTo}"/> into Storage with direction From -> To.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <param name="expression">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance.</param>
    /// <example>
    /// <code>
    /// mappings.Add&lt;Address, AddressModel&gt;(x => new AddressModel
    /// {
    ///     Id = x.Id,
    ///     BuildingNumber = x.BuildingNumber,
    ///     City = x.City,
    ///     State = x.State,
    ///     Country = (Countries) x.Country,
    ///     Street = x.Street,
    ///     ZipCode = x.ZipCode
    /// });
    /// </code>
    /// </example>
    public void Add<TFrom, TTo>(Expression<Func<TFrom, TTo>> expression)
    {
        Add(null, expression);
    }

    /// <summary>
    /// Adds Named <see cref="ExpressionMapping{TFrom, TTo}"/> into Storage with direction From -> To and explicitly specified name. 
    /// This mapping can be found only if name is specified explicitly into Map() method.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <param name="expression">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance.</param>
    /// <example>
    /// <code>
    /// mappings.Add&lt;Address, AddressModel&gt;("AdressMapping_1", x => new AddressModel
    /// {
    ///     Id = x.Id,
    ///     BuildingNumber = x.BuildingNumber,
    ///     City = x.City,
    ///     State = x.State,
    ///     Country = (Countries) x.Country,
    ///     Street = x.Street,
    ///     ZipCode = x.ZipCode
    /// });
    /// </code>
    /// </example>
    public void Add<TFrom, TTo>(string? name, Expression<Func<TFrom, TTo>> expression)
    {
        var mapping = new ExpressionMapping<TFrom, TTo>(expression, name);
        Add(mapping);
    }

    /// <summary>
    /// Adds <see cref="ParameterizedMapping{TFrom, TTo, TArgs}"/> into Storage with direction From -> To.
    /// Expression of this mapping contains imput arguments that can be used inside of mapping expression. 
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="expression">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance. Input parameter is an object of arguments that will be used inside of expression.</param>
    /// <example>
    /// <code>
    /// mappings.Add&lt;Appartment, AppartmentModel, AppartmentsArguments&gt;(args =>
    /// {
    ///     return x => new AppartmentModel
    ///     {
    ///         Id = x.Id,
    ///         Badrooms = x.Badrooms,
    ///         Bathrooms = x.Bathrooms,
    ///         Floor = x.Floor,
    ///         IsLodge = x.IsLodge,
    ///         Number = x.Number,
    ///         Size = x.Size.ToString() + args.UnitOfMeasure
    ///     };
    /// });
    /// </code>
    /// </example>
    public void Add<TFrom, TTo, TArgs>(Func<TArgs, Expression<Func<TFrom, TTo>>> expression)
    {
        Add(null, expression);
    }

    /// <summary>
    /// Adds Named <see cref="ParameterizedMapping{TFrom, TTo, TArgs}"/> into Storage with direction From -> To and explicitly specified name.
    /// Expression of this mapping contains imput arguments that can be used inside of mapping expression. 
    /// This mapping can be found only if name is specified explicitly into Map() method.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <param name="expression">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance. Input parameter is an object of arguments that will be used inside of expression.</param>
    /// <example>
    /// <code>
    /// mappings.Add&lt;Appartment, AppartmentModel, AppartmentsArguments&gt;(MappingsNames.AppartmentsWithoutBuilding, args =>
    /// {
    ///     return x => new AppartmentModel
    ///     {
    ///         Id = x.Id,
    ///         Badrooms = x.Badrooms,
    ///         Bathrooms = x.Bathrooms,
    ///         Floor = x.Floor,
    ///         IsLodge = x.IsLodge,
    ///         Number = x.Number,
    ///         Size = x.Size.ToString() + args.UnitOfMeasure
    ///     };
    /// });
    /// </code>
    /// </example>
    public void Add<TFrom, TTo, TArgs>(string? name, Func<TArgs, Expression<Func<TFrom, TTo>>> expression)
    {
        var mapping = new ParameterizedMapping<TFrom, TTo, TArgs>(expression, name);
        Add(mapping);
    }

    /// <summary>
    /// Adds Named <see cref="QueryMapping{TFrom, TTo, TContext}"/> into Storage with direction From -> To.
    /// Expression of this mapping contains EF Context as input parameter. EF Context will be injected into mapping expression.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <typeparam name="TContext">Interface that is implemeted by EF Context or EF Context type itself.</typeparam>
    /// <param name="expression">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance. Input parameters contain EF Context that can be used inside of mapping.</param>
    /// <example>
    /// <code>
    /// mappings.Add&lt;Appartment, AppartmentReviewsModel, IDbContext&gt;((query, context) =>
    ///     from appartment in query
    ///     join review in context.Set&lt;Review&gt;() on new { EntityId = appartment.Id, EntityTypeId = (int) EntityType.Appartment }
    ///                                          equals new { EntityId = review.EntityId, EntityTypeId = review.EntityTypeId }
    ///                                          into reviews
    ///     select new AppartmentReviewsModel
    ///     {
    ///         Id = appartment.Id,
    ///         Number = appartment.Number,
    ///         Reviews = reviews.Select(review => new ReviewModel
    ///         {
    ///             Id = review.Id,
    ///             EntityId = review.EntityId,
    ///             EntityType = (EntityType) review.EntityTypeId,
    ///             Rating = review.Rating,
    ///             Comments = review.Comments
    ///         }).ToList()
    ///     });
    /// </code>
    /// </example>
    public void Add<TFrom, TTo, TContext>(Func<IQueryable<TFrom>, TContext, IQueryable<TTo>> expression)
    {
        Add(null, expression);
    }

    /// <summary>
    /// Adds Named <see cref="QueryMapping{TFrom, TTo, TContext}"/> into Storage with direction From -> To and explicitly specified name.
    /// Expression of this mapping contains EF Context as input parameter. EF Context will be injected into mapping expression.
    /// This mapping can be found only if name is specified explicitly into Map() method.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <typeparam name="TContext">Interface that is implemeted by EF Context or EF Context type itself.</typeparam>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <param name="expression">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance. Input parameters contain EF Context that can be used inside of mapping.</param>
    /// <example>
    /// <code>
    /// mappings.Add&lt;Appartment, AppartmentReviewsModel, IDbContext&gt;("Mapping_2", (query, context) =>
    ///     from appartment in query
    ///     join review in context.Set&lt;Review&gt;() on new { EntityId = appartment.Id, EntityTypeId = (int) EntityType.Appartment }
    ///                                          equals new { EntityId = review.EntityId, EntityTypeId = review.EntityTypeId }
    ///                                          into reviews
    ///     select new AppartmentReviewsModel
    ///     {
    ///         Id = appartment.Id,
    ///         Number = appartment.Number,
    ///         Reviews = reviews.Select(review => new ReviewModel
    ///         {
    ///             Id = review.Id,
    ///             EntityId = review.EntityId,
    ///             EntityType = (EntityType) review.EntityTypeId,
    ///             Rating = review.Rating,
    ///             Comments = review.Comments
    ///         }).ToList()
    ///     });
    /// </code>
    /// </example>
    public void Add<TFrom, TTo, TContext>(string? name, Func<IQueryable<TFrom>, TContext, IQueryable<TTo>> expression)
    {
        var mapping = new QueryMapping<TFrom, TTo, TContext>(expression, name);
        Add(mapping);
    }

    /// <summary>
    /// Adds <see cref="ParameterizedQueryMapping{TFrom, TTo, TArgs, TContext}"/> into Storage with direction From -> To.
    /// Expression of this mapping contains imput arguments that can be used inside of mapping expression.
    /// Also expression of this mapping contains EF Context as input parameter. EF Context will be injected into mapping expression.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <typeparam name="TContext">Interface that is implemeted by EF Context or EF Context type itself.</typeparam>
    /// <param name="expression">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance. Input parameters contain arguments and EF Context that can be used inside of mapping.</param>
    /// <example>
    /// <code>
    /// mappings.Add&lt;Building, BuildingStatisticsModel, BuildingArguments, IDbContext&gt;(args =>
    /// {
    ///     return (query, context) =>
    ///         from building in query
    ///         join review in context.Set&lt;Review&gt;() on new { EntityId = building.Id, EntityTypeId = (int) EntityType.Building }
    ///                                              equals new { EntityId = review.EntityId, EntityTypeId = review.EntityTypeId }
    ///                                              into reviews
    ///         let address = building.Address
    ///         select new BuildingStatisticsModel
    ///         {
    ///             Id = building.Id,
    ///             Address = address.BuildingNumber + ", " + address.Street + ", " + address.City,
    ///             AppartmentsCount = building.Appartments.Count(),
    ///             Size = building.Appartments.Sum(app => app.Size),
    ///             ResidentsCount = building.Appartments.SelectMany(app => app.Residents).Where(r => r.Age > args.TargetResidentsAge).Count(),
    ///             AverageBuildingRating = reviews.Average(r => r.Rating)
    ///         };
    /// });
    /// </code>
    /// </example>
    public void Add<TFrom, TTo, TArgs, TContext>(Func<TArgs, Func<IQueryable<TFrom>, TContext, IQueryable<TTo>>> expression)
    {
        Add(null, expression);
    }

    /// <summary>
    /// Adds Named <see cref="ParameterizedQueryMapping{TFrom, TTo, TArgs, TContext}"/> into Storage with direction From -> To and explicitly specified name.
    /// Expression of this mapping contains imput arguments that can be used inside of mapping expression.
    /// Also expression of this mapping contains EF Context as input parameter. EF Context will be injected into mapping expression.
    /// This mapping can be found only if name is specified explicitly into Map() method.
    /// </summary>
    /// <typeparam name="TFrom">Source type of mapping.</typeparam>
    /// <typeparam name="TTo">Destination type of mapping.</typeparam>
    /// <typeparam name="TArgs">Type of arguments that we pass into mapping expression.</typeparam>
    /// <typeparam name="TContext">Interface that is implemeted by EF Context or EF Context type itself.</typeparam>
    /// <param name="name">Name of the mapping, if we want to search for mapping registered with some specific name. Should be null if we want to find mapping without name.</param>
    /// <param name="expression">Mapping expression that will be applied on <see cref="IQueryable{T}"/> instance. Input parameters contain arguments and EF Context that can be used inside of mapping.</param>
    /// <example>
    /// <code>
    /// mappings.Add&lt;Building, BuildingStatisticsModel, BuildingArguments, IDbContext&gt;("MappingName", args =>
    /// {
    ///     return (query, context) =>
    ///         from building in query
    ///         join review in context.Set&lt;Review&gt;() on new { EntityId = building.Id, EntityTypeId = (int) EntityType.Building }
    ///                                              equals new { EntityId = review.EntityId, EntityTypeId = review.EntityTypeId }
    ///                                              into reviews
    ///         let address = building.Address
    ///         select new BuildingStatisticsModel
    ///         {
    ///             Id = building.Id,
    ///             Address = address.BuildingNumber + ", " + address.Street + ", " + address.City,
    ///             AppartmentsCount = building.Appartments.Count(),
    ///             Size = building.Appartments.Sum(app => app.Size),
    ///             ResidentsCount = building.Appartments.SelectMany(app => app.Residents).Where(r => r.Age > args.TargetResidentsAge).Count(),
    ///             AverageBuildingRating = reviews.Average(r => r.Rating)
    ///         };
    /// });
    /// </code>
    /// </example>
    public void Add<TFrom, TTo, TArgs, TContext>(string? name, Func<TArgs, Func<IQueryable<TFrom>, TContext, IQueryable<TTo>>> expression)
    {
        var mapping = new ParameterizedQueryMapping<TFrom, TTo, TArgs, TContext>(expression, name);
        Add(mapping);
    }

    /// <summary>
    /// Removes all mappings from In-Memory storage.
    /// </summary>
    public void Clear()
    {
        Mappings.Clear();
    }
}
