using DevTeam.Extensions.Abstractions;
using System;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DevTeam.GenericService;

public partial class GenericService<TContext>
{
    #region Get One

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel, TKey>(TKey id, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        var query = _readRepository.QueryOne<TEntity, TKey>(id);
        return _mappings.Map<TEntity, TModel>(query, mappingName);
    }

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : IServiceArgs
    {
        var query = args.Type == ArgumentType.Mapping
            ? _readRepository.QueryOne<TEntity, TKey>(id)
            : _readRepository.QueryOne<TEntity, TKey, TArgs>(id, args);

        return args.Type == ArgumentType.Query
            ? _mappings.Map<TEntity, TModel>(query, mappingName)
            : _mappings.Map<TEntity, TModel, TArgs>(query, args, mappingName);
    }

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel>(int id, string? mappingName = null)
        where TEntity : class, IEntity
    {
        return QueryOne<TEntity, TModel, int>(id, mappingName);
    }

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel, TArgs>(int id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity
        where TArgs : IServiceArgs
    {
        return QueryOne<TEntity, TModel, int, TArgs>(id, args, mappingName);
    }

    public virtual TModel Get<TEntity, TModel>(Expression<Func<TEntity, bool>> filter, string? mappingName = null)
        where TEntity : class
    {
        return QueryList<TEntity, TModel>(filter, mappingName).FirstOrDefault();
    }

    public virtual TModel Get<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(filter, args, mappingName).FirstOrDefault();
    }

    public virtual Task<TModel> GetAsync<TEntity, TModel>(Expression<Func<TEntity, bool>> filter, string? mappingName = null)
        where TEntity : class
    {
        return QueryList<TEntity, TModel>(filter, mappingName).FirstOrDefaultAsync();
    }

    public virtual Task<TModel> GetAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(filter, args, mappingName).FirstOrDefaultAsync();
    }

    public virtual TModel Get<TEntity, TModel, TKey>(TKey id, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return QueryOne<TEntity, TModel, TKey>(id, mappingName).FirstOrDefault();
    }

    public virtual TModel Get<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : IServiceArgs
    {
        return QueryOne<TEntity, TModel, TKey, TArgs>(id, args, mappingName).FirstOrDefault();
    }

    public virtual Task<TModel> GetAsync<TEntity, TModel, TKey>(TKey id, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return QueryOne<TEntity, TModel, TKey>(id, mappingName).FirstOrDefaultAsync();
    }

    public virtual Task<TModel> GetAsync<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : IServiceArgs
    {
        return QueryOne<TEntity, TModel, TKey, TArgs>(id, args, mappingName).FirstOrDefaultAsync();
    }

    public virtual TModel Get<TEntity, TModel>(int id, string? mappingName = null)
        where TEntity : class, IEntity
    {
        return QueryOne<TEntity, TModel>(id, mappingName).FirstOrDefault();
    }

    public virtual TModel Get<TEntity, TModel, TArgs>(int id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity
        where TArgs : IServiceArgs
    {
        return QueryOne<TEntity, TModel, TArgs>(id, args, mappingName).FirstOrDefault();
    }

    public virtual Task<TModel> GetAsync<TEntity, TModel>(int id, string? mappingName = null)
        where TEntity : class, IEntity
    {
        return QueryOne<TEntity, TModel>(id, mappingName).FirstOrDefaultAsync();
    }

    public virtual Task<TModel> GetAsync<TEntity, TModel, TArgs>(int id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity
        where TArgs : IServiceArgs
    {
        return QueryOne<TEntity, TModel, TArgs>(id, args, mappingName).FirstOrDefaultAsync();
    }

    #endregion

    #region Get Property

    public virtual TProperty GetProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                             Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class
    {
        return _readRepository.GetProperty(filter, selector);
    }

    public virtual TProperty GetProperty<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                                    Expression<Func<TEntity, TProperty>> selector,
                                                                    TArgs args)
        where TEntity : class
    {
        return _readRepository.GetProperty(filter, selector, args);
    }

    public virtual Task<TProperty> GetPropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                                        Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class
    {
        return _readRepository.GetPropertyAsync(filter, selector);
    }

    public virtual Task<TProperty> GetPropertyAsync<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                                               Expression<Func<TEntity, TProperty>> selector,
                                                                               TArgs args)
        where TEntity : class
    {
        return _readRepository.GetPropertyAsync(filter, selector, args);
    }

    public virtual TProperty GetProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return _readRepository.GetProperty(id, selector);
    }

    public virtual TProperty GetProperty<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return _readRepository.GetProperty(id, selector, args);
    }

    public virtual Task<TProperty> GetPropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return _readRepository.GetPropertyAsync(id, selector);
    }

    public virtual Task<TProperty> GetPropertyAsync<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return _readRepository.GetPropertyAsync(id, selector, args);
    }

    public virtual TProperty GetProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity
    {
        return _readRepository.GetProperty(id, selector);
    }

    public virtual TProperty GetProperty<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity
    {
        return _readRepository.GetProperty(id, selector, args);
    }

    public virtual Task<TProperty> GetPropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity
    {
        return _readRepository.GetPropertyAsync(id, selector);
    }

    public virtual Task<TProperty> GetPropertyAsync<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity
    {
        return _readRepository.GetPropertyAsync(id, selector, args);
    }

    #endregion

    #region Any

    public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
        where TEntity : class
    {
        return _readRepository.Any(filter);
    }

    public virtual bool Any<TEntity, TArgs>(TArgs args)
        where TEntity : class
    {
        return _readRepository.Any<TEntity, TArgs>(args);
    }

    public virtual bool Any<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class
    {
        return _readRepository.Any(filter, args);
    }

    public virtual Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
        where TEntity : class
    {
        return _readRepository.AnyAsync(filter);
    }

    public virtual Task<bool> AnyAsync<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class
    {
        return _readRepository.AnyAsync(filter, args);
    }

    public virtual Task<bool> AnyAsync<TEntity, TArgs>(TArgs args)
        where TEntity : class
    {
        return _readRepository.AnyAsync<TEntity, TArgs>(args);
    }

    #endregion

}
