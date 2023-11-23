using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using DevTeam.Extensions.Abstractions;
using DevTeam.Extensions.EntityFrameworkCore;

namespace DevTeam.GenericRepository;

public interface IReadOnlyRepository<TContext, TOptions> : IRepository<TContext, TOptions>
        where TContext : IDbContext
        where TOptions : QueryOptions
{ }

public interface IReadOnlyRepository : IReadOnlyRepository<IDbContext, QueryOptions>
{ }

public interface IReadOnlyRepository<TOptions> : IReadOnlyRepository<IDbContext, TOptions>
    where TOptions : QueryOptions, new()
{ }

public interface ISoftDeleteRepository<TContext, TOptions> : IRepository<TContext, TOptions>
    where TContext : IDbContext
    where TOptions : QueryOptions, new()
{ }

public interface ISoftDeleteRepository : ISoftDeleteRepository<IDbContext, QueryOptions>
{ }

public interface ISoftDeleteRepository<TOptions> : ISoftDeleteRepository<IDbContext, TOptions>
    where TOptions : QueryOptions, new()
{ }

public interface IReadOnlyDeleteRepository<TContext, TOptions> : IReadOnlyRepository<TContext, TOptions>
    where TContext : IDbContext
    where TOptions : QueryOptions, new()
{ }

public interface IReadOnlyDeleteRepository : IReadOnlyDeleteRepository<IDbContext, QueryOptions>
{ }

public interface IReadOnlyDeleteRepository<TOptions> : IReadOnlyDeleteRepository<IDbContext, TOptions>
    where TOptions : QueryOptions, new()
{ }

public interface IRepository : IRepository<IDbContext, QueryOptions>
{ }

public interface IRepository<TOptions> : IRepository<IDbContext, TOptions>
    where TOptions : QueryOptions
{ }

public interface IRepository<TContext, TOptions>
    where TContext : IDbContext
    where TOptions : QueryOptions
{
    IQueryable<TEntity> Query<TEntity>(TOptions? options = null)
        where TEntity : class;
    IQueryable<TEntity> Query<TEntity, TArgs>(TArgs args, TOptions? options = null)
        where TEntity : class;
    IQueryable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>>? filter = null, TOptions? options = null)
        where TEntity : class;
    IQueryable<TEntity> GetList<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class;
    IQueryable<TEntity> QueryOne<TEntity, TKey>(TKey id, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    IQueryable<TEntity> QueryOne<TEntity, TKey, TArgs>(TKey id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    IQueryable<TEntity> QueryOne<TEntity>(int id, TOptions? options = null)
        where TEntity : class, IEntity;
    IQueryable<TEntity> QueryOne<TEntity, TArgs>(int id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity;
    TEntity? Get<TEntity>(Expression<Func<TEntity, bool>> filter, TOptions? options = null)
        where TEntity : class;
    TEntity? Get<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class;
    Task<TEntity?> GetAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class;
    Task<TEntity?> GetAsync<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class;
    TEntity? Get<TEntity, TKey>(TKey id)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    TEntity? Get<TEntity, TKey, TArgs>(TKey id, TArgs args)
       where TEntity : class, IEntity<TKey>
       where TKey : IEquatable<TKey>;
    Task<TEntity?> GetAsync<TEntity, TKey>(TKey id)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task<TEntity?> GetAsync<TEntity, TKey, TArgs>(TKey id, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    TEntity? Get<TEntity>(int id)
        where TEntity : class, IEntity;
    TEntity? Get<TEntity, TArgs>(int id, TArgs args)
        where TEntity : class, IEntity;
    Task<TEntity?> GetAsync<TEntity>(int id)
        where TEntity : class, IEntity;
    Task<TEntity?> GetAsync<TEntity, TArgs>(int id, TArgs args)
        where TEntity : class, IEntity;
    TProperty? GetProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                              Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class;
    TProperty? GetProperty<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                     Expression<Func<TEntity, TProperty>> selector,
                                                     TArgs args)
        where TEntity : class;
    Task<TProperty?> GetPropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> filter,
                                                         Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class;
    Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TArgs>(Expression<Func<TEntity, bool>> filter,
                                                                Expression<Func<TEntity, TProperty>> selector,
                                                                TArgs args)
        where TEntity : class;
    TProperty? GetProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    TProperty? GetProperty<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TKey, TArgs>(TKey id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    TProperty? GetProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity;
    TProperty? GetProperty<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity;
    Task<TProperty?> GetPropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> selector)
        where TEntity : class, IEntity;
    Task<TProperty?> GetPropertyAsync<TEntity, TProperty, TArgs>(int id, Expression<Func<TEntity, TProperty>> selector, TArgs args)
        where TEntity : class, IEntity;
    bool Any<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
        where TEntity : class;
    bool Any<TEntity, TArgs>(TArgs args)
        where TEntity : class;
    bool Any<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class;
    Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
        where TEntity : class;
    Task<bool> AnyAsync<TEntity, TArgs>(TArgs args)
        where TEntity : class;
    Task<bool> AnyAsync<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class;
    TEntity Add<TEntity>(TEntity entity)
        where TEntity : class;
    Task<TEntity> AddAsync<TEntity>(TEntity entity)
        where TEntity : class;
    List<TEntity> AddRange<TEntity>(List<TEntity> entities)
        where TEntity : class;
    Task<List<TEntity>> AddRangeAsync<TEntity>(List<TEntity> entities)
        where TEntity : class;
    void Update<TEntity>(TEntity entity)
        where TEntity : class;
    void UpdateProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> selector,
                                            Expression<Func<TEntity, TProperty>> propertySelector,
                                            TProperty value)
        where TEntity : class;
    void UpdateProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    void UpdateProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity;
    void UpdateProperty<TEntity>(Expression<Func<TEntity, bool>> selector, string propertyName, object value)
        where TEntity : class;
    void UpdateProperty<TEntity, TKey>(TKey id, string propertyName, object value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    void UpdateProperty<TEntity>(int id, string propertyName, object value)
        where TEntity : class, IEntity;
    Task UpdatePropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> selector,
                                                           Expression<Func<TEntity, TProperty>> propertySelector,
                                                           TProperty value)
        where TEntity : class;
    Task UpdatePropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task UpdatePropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity;
    Task UpdatePropertyAsync<TEntity>(Expression<Func<TEntity, bool>> selector, string propertyName, object value)
        where TEntity : class;
    Task UpdatePropertyAsync<TEntity, TKey>(TKey id, string propertyName, object value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task UpdatePropertyAsync<TEntity>(int id, string propertyName, object value)
        where TEntity : class, IEntity;
    void Delete<TEntity>(int id)
        where TEntity : class, IEntity;
    Task DeleteAsync<TEntity>(int id)
        where TEntity : class, IEntity;
    void Delete<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class;
    Task DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class;
    void Delete<TEntity>(TEntity? entity)
        where TEntity : class;
    void DeleteRange<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class;
    Task DeleteRangeAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class;
    void DeleteRange<TEntity>(List<TEntity> entities)
        where TEntity : class;
    int Save();
    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
