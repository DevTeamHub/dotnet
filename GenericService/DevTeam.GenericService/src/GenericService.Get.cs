using DevTeam.Extensions.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevTeam.GenericService;

public partial class GenericService<TContext, TOptions>
{
    #region Get One

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel, TKey>(TKey id, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        var query = _readRepository.QueryOne<TEntity, TKey>(id, options);
        return _mappings.Map<TEntity, TModel>(query, mappingName);
    }

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IServiceArgs
    {
        var query = args.Type == ArgumentType.Mapping
            ? _readRepository.QueryOne<TEntity, TKey>(id, options)
            : _readRepository.QueryOne<TEntity, TKey, TArgs>(id, args, options);

        return args.Type == ArgumentType.Query
            ? _mappings.Map<TEntity, TModel>(query, mappingName)
            : _mappings.Map<TEntity, TModel, TArgs>(query, args, mappingName);
    }

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel>(int id, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity
    {
        return QueryOne<TEntity, TModel, int>(id, mappingName, options);
    }

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel, TArgs>(int id, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IServiceArgs
    {
        return QueryOne<TEntity, TModel, int, TArgs>(id, args, mappingName, options);
    }

    public virtual TModel? Get<TEntity, TModel>(Expression<Func<TEntity, bool>> filter, string? mappingName = null, TOptions? options = null)
        where TEntity : class
    {
        return QueryList<TEntity, TModel>(filter, mappingName, options).FirstOrDefault();
    }

    public virtual TModel? Get<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(filter, args, mappingName, options).FirstOrDefault();
    }

    public virtual Task<TModel?> GetAsync<TEntity, TModel>(Expression<Func<TEntity, bool>> filter, string? mappingName = null, TOptions? options = null)
        where TEntity : class
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return QueryList<TEntity, TModel>(filter, mappingName, options).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual Task<TModel?> GetAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IServiceArgs
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return QueryList<TEntity, TModel, TArgs>(filter, args, mappingName, options).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual TModel? Get<TEntity, TModel, TKey>(TKey id, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return QueryOne<TEntity, TModel, TKey>(id, mappingName, options).FirstOrDefault();
    }

    public virtual TModel? Get<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IServiceArgs
    {
        return QueryOne<TEntity, TModel, TKey, TArgs>(id, args, mappingName, options).FirstOrDefault();
    }

    public virtual Task<TModel?> GetAsync<TEntity, TModel, TKey>(TKey id, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return QueryOne<TEntity, TModel, TKey>(id, mappingName, options).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual Task<TModel?> GetAsync<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IServiceArgs
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return QueryOne<TEntity, TModel, TKey, TArgs>(id, args, mappingName, options).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual TModel? Get<TEntity, TModel>(int id, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity
    {
        return QueryOne<TEntity, TModel>(id, mappingName, options).FirstOrDefault();
    }

    public virtual TModel? Get<TEntity, TModel, TArgs>(int id, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IServiceArgs
    {
        return QueryOne<TEntity, TModel, TArgs>(id, args, mappingName, options).FirstOrDefault();
    }

    public virtual Task<TModel?> GetAsync<TEntity, TModel>(int id, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return QueryOne<TEntity, TModel>(id, mappingName, options).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual Task<TModel?> GetAsync<TEntity, TModel, TArgs>(int id, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IServiceArgs
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return QueryOne<TEntity, TModel, TArgs>(id, args, mappingName, options).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    #endregion

    #region Get Property

    public virtual TProperty? GetProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                             Expression<Func<TEntity, TProperty>> selector,
                                                             TOptions? options = null)
        where TEntity : class
    {
        return _readRepository.GetProperty(filter, selector, options);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                                    Expression<Func<TEntity, TProperty>> selector,
                                                                    TArgs args,
                                                                    TOptions? options = null)
        where TEntity : class
        where TArgs: class, IServiceArgs
    {
        return _readRepository.GetProperty(filter, selector, args, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                                        Expression<Func<TEntity, TProperty>> selector,
                                                                        TOptions? options = null)
        where TEntity : class
    {
        return _readRepository.GetPropertyAsync(filter, selector, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                                               Expression<Func<TEntity, TProperty>> selector,
                                                                               TArgs args,
                                                                               TOptions? options = null)
        where TEntity : class
        where TArgs : class, IServiceArgs
    {
        return _readRepository.GetPropertyAsync(filter, selector, args, options);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return _readRepository.GetProperty(id, selector, options);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IServiceArgs
    {
        return _readRepository.GetProperty(id, selector, args, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return _readRepository.GetPropertyAsync(id, selector, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IServiceArgs
    {
        return _readRepository.GetPropertyAsync(id, selector, args, options);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector, TOptions? options = null)
        where TEntity : class, IEntity
    {
        return _readRepository.GetProperty(id, selector, options);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IServiceArgs
    {
        return _readRepository.GetProperty(id, selector, args, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector, TOptions? options = null)
        where TEntity : class, IEntity
    {
        return _readRepository.GetPropertyAsync(id, selector, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IServiceArgs
    {
        return _readRepository.GetPropertyAsync(id, selector, args, options);
    }

    #endregion

    #region Any

    public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>>? filter = null, TOptions? options = null)
        where TEntity : class
    {
        return _readRepository.Any(filter, options);
    }

    public virtual bool Any<TEntity, TArgs>(TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IServiceArgs
    {
        return _readRepository.Any<TEntity, TArgs>(args, options);
    }

    public virtual bool Any<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IServiceArgs
    {
        return _readRepository.Any(filter, args, options);
    }

    public virtual Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null, TOptions? options = null)
        where TEntity : class
    {
        return _readRepository.AnyAsync(filter, options);
    }

    public virtual Task<bool> AnyAsync<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IServiceArgs
    {
        return _readRepository.AnyAsync(filter, args, options);
    }

    public virtual Task<bool> AnyAsync<TEntity, TArgs>(TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IServiceArgs
    {
        return _readRepository.AnyAsync<TEntity, TArgs>(args, options);
    }

    #endregion

}
