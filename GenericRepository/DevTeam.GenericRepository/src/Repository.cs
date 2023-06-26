using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DevTeam.Extensions.Abstractions;
using DevTeam.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevTeam.GenericRepository;

public class Repository : Repository<IDbContext>, IRepository
{
    public Repository(IDbContext context)
        : base(context)
    { }
}

public class Repository<TContext> : IRepository<TContext>
    where TContext : IDbContext
{
    protected readonly TContext Context;

    public Repository(TContext context)
    {
        Context = context;
    }

    protected virtual IQueryable<TEntity> GetQuery<TEntity>()
        where TEntity : class
    {
        return Context.Set<TEntity>().AsQueryable();
    }

    protected virtual IQueryable<TEntity> GetQuery<TEntity, TArgs>(TArgs args)
        where TEntity : class
    {
        return GetQuery<TEntity>();
    }

    private static IQueryable<TEntity> InternalQuery<TEntity>(IQueryable<TEntity> query)
    {
        if (typeof(IDeleted).IsAssignableFrom(typeof(TEntity)))
        {
            query = ((IQueryable<IDeleted>)query).Where(x => !x.IsDeleted).Cast<TEntity>();
        }

        return query;
    }

    public virtual IQueryable<TEntity> Query<TEntity>()
        where TEntity : class
    {
        var query = GetQuery<TEntity>();
        return InternalQuery(query);
    }

    public virtual IQueryable<TEntity> Query<TEntity, TArgs>(TArgs args)
        where TEntity : class
    {
        var query = GetQuery<TEntity, TArgs>(args);
        return InternalQuery(query);
    }

    public virtual IQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
        where TEntity : class
    {
        var query = Query<TEntity>();

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return query;
    }

    public virtual IQueryable<TEntity> GetList<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class
    {
        return Query<TEntity, TArgs>(args).Where(filter);
    }

    public virtual IQueryable<TEntity> QueryOne<TEntity, TKey>(TKey id)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetList<TEntity>(x => x.Id.Equals(id));
    }

    public virtual IQueryable<TEntity> QueryOne<TEntity, TKey, TArgs>(TKey id, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetList<TEntity, TArgs>(x => x.Id.Equals(id), args);
    }

    public virtual IQueryable<TEntity> QueryOne<TEntity>(int id)
        where TEntity : class, IEntity
    {
        return QueryOne<TEntity, int>(id);
    }

    public virtual IQueryable<TEntity> QueryOne<TEntity, TArgs>(int id, TArgs args)
        where TEntity : class, IEntity
    {
        return QueryOne<TEntity, int, TArgs>(id, args);
    }

    public virtual TEntity? Get<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        return GetList(filter).FirstOrDefault();
    }

    public virtual TEntity? Get<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class
    {
        return GetList(filter, args).FirstOrDefault();
    }

    public virtual Task<TEntity?> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return GetList(filter).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual Task<TEntity?> GetAsync<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return GetList(filter, args).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual TEntity? Get<TEntity, TKey>(TKey id)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return Get<TEntity>(x => x.Id.Equals(id));
    }

    public virtual TEntity? Get<TEntity, TKey, TArgs>(TKey id, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return Get<TEntity, TArgs>(x => x.Id.Equals(id), args);
    }

    public virtual Task<TEntity?> GetAsync<TEntity, TKey>(TKey id)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetAsync<TEntity>(x => x.Id.Equals(id));
    }

    public virtual Task<TEntity?> GetAsync<TEntity, TKey, TArgs>(TKey id, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetAsync<TEntity, TArgs>(x => x.Id.Equals(id), args);
    }

    public virtual TEntity? Get<TEntity>(int id)
        where TEntity : class, IEntity
    {
        return Get<TEntity, int>(id);
    }

    public virtual TEntity? Get<TEntity, TArgs>(int id, TArgs args)
        where TEntity : class, IEntity
    {
        return Get<TEntity, int, TArgs>(id, args);
    }

    public virtual Task<TEntity?> GetAsync<TEntity>(int id)
        where TEntity : class, IEntity
    {
        return GetAsync<TEntity, int>(id);
    }

    public virtual Task<TEntity?> GetAsync<TEntity, TArgs>(int id, TArgs args)
        where TEntity : class, IEntity
    {
        return GetAsync<TEntity, int, TArgs>(id, args);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                             Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class
    {
        return GetList(filter).Select(selector).FirstOrDefault();
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                                     Expression<Func<TEntity, TProperty>> selector,
                                                                     TArgs args)
        where TEntity : class
    {
        return GetList(filter, args).Select(selector).FirstOrDefault();
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                                         Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return GetList(filter).Select(selector).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                                                Expression<Func<TEntity, TProperty>> selector,
                                                                                TArgs args)
        where TEntity : class
    {
#pragma warning disable CS8619 // Nullability of reference types in value doesn't match target type.
        return GetList(filter, args).Select(selector).FirstOrDefaultAsync();
#pragma warning restore CS8619 // Nullability of reference types in value doesn't match target type.
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetProperty(x => x.Id.Equals(id), selector);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetProperty(x => x.Id.Equals(id), selector, args);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetPropertyAsync(x => x.Id.Equals(id), selector);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return GetPropertyAsync(x => x.Id.Equals(id), selector, args);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity
    {
        return GetProperty<TEntity, TProperty, int>(id, selector);
    }

    public virtual TProperty? GetProperty<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity
    {
        return GetProperty<TEntity, TProperty, int, TArgs>(id, selector, args);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity
    {
        return GetPropertyAsync<TEntity, TProperty, int>(id, selector);
    }

    public virtual Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity
    {
        return GetPropertyAsync<TEntity, TProperty, int, TArgs>(id, selector, args);
    }

    public virtual bool Any<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
        where TEntity : class
    {
        var query = Query<TEntity>();
        return filter != null ? query.Any(filter) : query.Any();
    }

    public virtual bool Any<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class
    {
        return Query<TEntity, TArgs>(args).Any(filter);
    }

    public virtual bool Any<TEntity, TArgs>(TArgs args)
        where TEntity : class
    {
        return Query<TEntity, TArgs>(args).Any();
    }

    public virtual Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
        where TEntity : class
    {
        var query = Query<TEntity>();
        return filter != null ? query.AnyAsync(filter) : query.AnyAsync();
    }

    public virtual Task<bool> AnyAsync<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class
    {
        return Query<TEntity, TArgs>(args).AnyAsync(filter);
    }

    public virtual Task<bool> AnyAsync<TEntity, TArgs>(TArgs args)
        where TEntity : class
    {
        return Query<TEntity, TArgs>(args).AnyAsync();
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

        if (item is IEntity entry)
        {
            var local = Context.Set<TEntity>().Local.Cast<IEntity>().FirstOrDefault(x => x.Id == entry.Id);
            if (local != null)
            {
                Context.Entry(local).CurrentValues.SetValues(entity);
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

    public virtual async Task UpdatePropertyAsync<TEntity>(Expression<Func<TEntity, bool>> selector, string propertyName, object value)
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
