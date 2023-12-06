using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DevTeam.Extensions.Abstractions;
using DevTeam.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DevTeam.GenericRepository;

public class Repository : Repository<IDbContext, QueryOptions>, IRepository
{
    public Repository(IDbContext context, IServiceProvider serviceProvider, QueryOptions? options = null)
        : base(context, serviceProvider, options)
    { }
}

public class Repository<TOptions> : Repository<IDbContext, TOptions>, IRepository<TOptions>
    where TOptions : QueryOptions, new()
{
    public Repository(IDbContext context, IServiceProvider serviceProvider, TOptions? options = null)
        : base(context, serviceProvider, options)
    { }
}

public class Repository<TContext, TOptions> : IRepository<TContext, TOptions>
    where TContext : IDbContext
    where TOptions : QueryOptions, new()
{
    protected readonly TContext Context;
    protected readonly TOptions DefaultOptions;
    private readonly IServiceProvider _serviceProvider;

    public Repository(TContext context, IServiceProvider serviceProvider, TOptions? options = null)
    {
        Context = context;
        DefaultOptions = options ?? new TOptions();
        _serviceProvider = serviceProvider;
    }

    public virtual IQueryable<TEntity> Query<TEntity>(TOptions? options = null)
        where TEntity : class
    {
        options ??= DefaultOptions;
        var query = Context.Set<TEntity>().AsQueryable();

        var queryExtensions = _serviceProvider
            .GetServices(typeof(IQueryExtension<TEntity, TOptions>))
            .Cast<IQueryExtension<TEntity, TOptions>>()
            .Where(x => x.CanApply(options))
            .OrderBy(x => x.Order)
            .ToList();    

        query = queryExtensions.Aggregate(query, (q, extension) => extension.ApplyExtension(q));

        return query;
    }

    public virtual IQueryable<TEntity> Query<TEntity, TArgs>(TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        options ??= DefaultOptions;
        var query = Query<TEntity>(options);

        var securityQueryExtensions = _serviceProvider
            .GetServices(typeof(ISecurityQueryExtension<TEntity, TOptions>))
            .Cast<ISecurityQueryExtension<TEntity, TOptions>>()
            .Where(x => x.CanApply(options))
            .OrderBy(x => x.Order)
            .ToList();

        return securityQueryExtensions.Aggregate(query, (q, extension) => extension.ApplyExtension(q, args));
    }

    public virtual IQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>>? filter = null, TOptions? options = null)
        where TEntity : class
    {
        var query = Query<TEntity>(options);

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query;
    }

    public virtual IQueryable<TEntity> GetList<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return Query<TEntity, TArgs>(args,options).Where(filter);
    }

    public virtual IQueryable<TEntity> QueryOne<TEntity, TKey>(TKey id, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetList<TEntity>(x => x.Id.Equals(id), options);
    }

    public virtual IQueryable<TEntity> QueryOne<TEntity, TKey, TArgs>(TKey id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return GetList<TEntity, TArgs>(x => x.Id.Equals(id), args, options);
    }

    public virtual IQueryable<TEntity> QueryOne<TEntity>(int id, TOptions? options = null)
        where TEntity : class, IEntity
    {
        return QueryOne<TEntity, int>(id, options);
    }

    public virtual IQueryable<TEntity> QueryOne<TEntity, TArgs>(int id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return QueryOne<TEntity, int, TArgs>(id, args, options);
    }

    public virtual TEntity? Get<TEntity>(Expression<Func<TEntity, bool>> filter, TOptions? options = null)
        where TEntity : class
    {
        return GetList(filter, options).FirstOrDefault();
    }

    public virtual TEntity? Get<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return GetList(filter, args, options).FirstOrDefault();

    }

    public virtual Task<TEntity?> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter, TOptions? options = null)
        where TEntity : class
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return GetList(filter, options).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual Task<TEntity?> GetAsync<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return GetList(filter, args, options).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual TEntity? Get<TEntity, TKey>(TKey id, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return Get<TEntity>(x => x.Id.Equals(id), options);
    }

    public virtual TEntity? Get<TEntity, TKey, TArgs>(TKey id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return Get<TEntity, TArgs>(x => x.Id.Equals(id), args, options);
    }

    public virtual Task<TEntity?> GetAsync<TEntity, TKey>(TKey id, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetAsync<TEntity>(x => x.Id.Equals(id), options);
    }

    public virtual Task<TEntity?> GetAsync<TEntity, TKey, TArgs>(TKey id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return GetAsync<TEntity, TArgs>(x => x.Id.Equals(id), args, options);
    }

    public virtual TEntity? Get<TEntity>(int id, TOptions? options = null)
        where TEntity : class, IEntity
    {
        return Get<TEntity, int>(id, options);
    }

    public virtual TEntity? Get<TEntity, TArgs>(int id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return Get<TEntity, int, TArgs>(id, args, options);
    }

    public virtual Task<TEntity?> GetAsync<TEntity>(int id, TOptions? options = null)
        where TEntity : class, IEntity
    {
        return GetAsync<TEntity, int>(id, options);
    }

    public virtual Task<TEntity?> GetAsync<TEntity, TArgs>(int id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return GetAsync<TEntity, int, TArgs>(id, args, options);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                             Expression<Func<TEntity, TProperty>> selector,
                                                             TOptions? options = null)
        where TEntity : class
    {
        return GetList(filter, options).Select(selector).FirstOrDefault();
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                                     Expression<Func<TEntity, TProperty>> selector,
                                                                     TArgs args,
                                                                     TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return GetList(filter, args, options).Select(selector).FirstOrDefault();
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                                         Expression<Func<TEntity, TProperty>> selector,
                                                                         TOptions? options = null)
        where TEntity : class
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return GetList(filter, options).Select(selector).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                                                Expression<Func<TEntity, TProperty>> selector,
                                                                                TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return GetList(filter, args, options).Select(selector).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetProperty(x => x.Id.Equals(id), selector, options);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return GetProperty(x => x.Id.Equals(id), selector, args, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetPropertyAsync(x => x.Id.Equals(id), selector, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return GetPropertyAsync(x => x.Id.Equals(id), selector, args, options);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector, TOptions? options = null)
        where TEntity : class, IEntity
    {
        return GetProperty<TEntity, TProperty, int>(id, selector, options);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return GetProperty<TEntity, TProperty, int, TArgs>(id, selector, args, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector, TOptions? options = null)
        where TEntity : class, IEntity
    {
        return GetPropertyAsync<TEntity, TProperty, int>(id, selector, options);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return GetPropertyAsync<TEntity, TProperty, int, TArgs>(id, selector, args, options);
    }

    public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>>? filter = null, TOptions? options = null)
        where TEntity : class
    {
        var query = Query<TEntity>(options);
        return filter != null ? query.Any(filter) : query.Any();
    }

    public virtual bool Any<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return Query<TEntity>(options).Any(filter);
    }

    public virtual bool Any<TEntity, TArgs>(TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return Query<TEntity>(options).Any();
    }

    public virtual Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null, TOptions? options = null)
        where TEntity : class
    {
        var query = Query<TEntity>(options);
        return filter != null ? query.AnyAsync(filter) : query.AnyAsync();
    }

    public virtual Task<bool> AnyAsync<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return Query<TEntity>(options).AnyAsync(filter);
    }

    public virtual Task<bool> AnyAsync<TEntity, TArgs>(TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return Query<TEntity>(options).AnyAsync();
    }

    public virtual TEntity Add<TEntity>(TEntity entity)
        where TEntity : class
    {
        Context.Set<TEntity>().Add(entity);
        return entity;
    }

    public virtual async Task<TEntity> AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await Context.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public virtual List<TEntity> AddRange<TEntity>(List<TEntity> entities)
        where TEntity : class
    {
        Context.Set<TEntity>().AddRange(entities);
        return entities;
    }

    public virtual async Task<List<TEntity>> AddRangeAsync<TEntity>(List<TEntity> entities)
        where TEntity : class
    {
        await Context.Set<TEntity>().AddRangeAsync(entities);
        return entities;
    }

    public virtual void Update<TEntity>(TEntity entity)
        where TEntity : class
    {
        var item = Context.Entry(entity);

        if (item.Entity is IEntity entry)
        {
            var local = Context.Set<TEntity>().Local.Cast<IEntity>().FirstOrDefault(x => x.Id == entry.Id);
            if (local != null)
            {
                Context.Entry(local).CurrentValues.SetValues(entry);
                return;
            }
        }

        item.State = EntityState.Modified;
    }

    public virtual void UpdateProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> selector,
                                                           Expression<Func<TEntity, TProperty>> propertySelector,
                                                           TProperty value)
        where TEntity : class
    {
        var entity = Get(selector);

        if (entity == null) return;

        Context.Entry(entity).Property(propertySelector).CurrentValue = value;
    }

    public virtual void UpdateProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        UpdateProperty(x => x.Id.Equals(id), propertySelector, value);
    }

    public virtual void UpdateProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity
    {

        UpdateProperty<TEntity, TProperty, int>(id, propertySelector, value);
    }

    public virtual void UpdateProperty<TEntity>(Expression<Func<TEntity, bool>> selector, string propertyName, object value)
        where TEntity : class
    {
        var entity = Get(selector);

        if (entity == null) return;

        Context.Entry(entity).Property(propertyName).CurrentValue = value;
    }

    public virtual void UpdateProperty<TEntity, TKey>(TKey id, string propertyName, object value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        UpdateProperty<TEntity>(x => x.Id.Equals(id), propertyName, value);
    }

    public virtual void UpdateProperty<TEntity>(int id, string propertyName, object value)
        where TEntity : class, IEntity
    {
        UpdateProperty<TEntity, int>(id, propertyName, value);
    }

    public virtual async Task UpdatePropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> selector,
                                                                      Expression<Func<TEntity, TProperty>> propertySelector,
                                                                      TProperty value)
        where TEntity : class
    {
        var entity = await GetAsync(selector);

        if (entity == null) return;

        Context.Entry(entity).Property(propertySelector).CurrentValue = value;
    }

    public virtual Task UpdatePropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return UpdatePropertyAsync(x => x.Id.Equals(id), propertySelector, value);
    }

    public virtual Task UpdatePropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity
    {
        return UpdatePropertyAsync<TEntity, TProperty, int>(id, propertySelector, value);
    }

    public virtual async Task UpdatePropertyAsync<TEntity>(Expression<Func<TEntity, bool>> selector, 
                                                           string propertyName, 
                                                           object value)
        where TEntity : class
    {
        var entity = await GetAsync(selector);

        if (entity == null) return;

        Context.Entry(entity).Property(propertyName).CurrentValue = value;
    }

    public virtual Task UpdatePropertyAsync<TEntity, TKey>(TKey id, string propertyName, object value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return UpdatePropertyAsync<TEntity>(x => x.Id.Equals(id), propertyName, value);
    }

    public virtual Task UpdatePropertyAsync<TEntity>(int id, string propertyName, object value)
        where TEntity : class, IEntity
    {
        return UpdatePropertyAsync<TEntity, int>(id, propertyName, value);
    }

    public virtual void Delete<TEntity>(int id)
        where TEntity : class, IEntity
    {
        var entity = Get<TEntity>(id);
        Delete(entity);
    }

    public virtual async Task DeleteAsync<TEntity>(int id)
        where TEntity : class, IEntity
    {
        var entity = await GetAsync<TEntity>(id);
        Delete(entity);
    }

    public virtual void Delete<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        var entity = Get(filter);
        Delete(entity);
    }

    public virtual async Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        var entity = await GetAsync(filter);
        Delete(entity);
    }

    public virtual void Delete<TEntity>(TEntity? entity)
        where TEntity : class
    {
        if (entity == null) return;

        if (entity is IDeleted deleted)
        {
            deleted.IsDeleted = true;
            Update(entity);
        }
        else
        {
            Context.Set<TEntity>().Remove(entity);
        }
    }

    public virtual void DeleteRange<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        var entities = GetList(filter).ToList();
        DeleteRange(entities);
    }

    public virtual async Task DeleteRangeAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        var entities = await GetList(filter).ToListAsync();
        DeleteRange(entities);
    }

    public virtual void DeleteRange<TEntity>(List<TEntity> entities)
        where TEntity : class
    {
        if (typeof(TEntity).IsAssignableFrom(typeof(IDeleted)))
        {
            entities.ForEach(entity =>
            {
                if (entity is IDeleted deleted)
                {
                    deleted.IsDeleted = true;
                    Update(entity);
                }
            });
        }
        else
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }
    }

    public virtual int Save()
    {
        return Context.SaveChanges();
    }

    public virtual Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return Context.SaveChangesAsync(cancellationToken);
    }
}
