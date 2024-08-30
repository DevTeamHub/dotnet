using DevTeam.Extensions.Abstractions;
using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericService.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevTeam.GenericService;

public interface ISoftDeleteGenericService : ISoftDeleteGenericService<IDbContext>
{ }

public interface ISoftDeleteGenericService<TContext> : IGenericService<TContext>
    where TContext : IDbContext
{ }

public interface IGenericService : IGenericService<IDbContext>
{ }

public interface IGenericService<TContext>
    where TContext : IDbContext
{
    IQueryable<TModel> QueryList<TEntity, TModel>(Expression<Func<TEntity, bool>>? filter = null, string? mappingName = null)
        where TEntity : class;
    IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs;
    IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs;
    List<TModel> GetList<TEntity, TModel>(Expression<Func<TEntity, bool>>? filter = null, string? mappingName = null)
        where TEntity : class;
    List<TModel> GetList<TEntity, TModel, TArgs>(TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs;
    List<TModel> GetList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs;
    Task<List<TModel>> GetListAsync<TEntity, TModel>(Expression<Func<TEntity, bool>>? filter = null, string? mappingName = null)
        where TEntity : class;
    Task<List<TModel>> GetListAsync<TEntity, TModel, TArgs>(TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs;
    Task<List<TModel>> GetListAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs;
    Task<List<TModel>> Search<TEntity, TModel, TSearchModel>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, string? mappingName = null)
        where TEntity : class
        where TSearchModel : PaginationParams;
    Task<List<TModel>> Search<TEntity, TModel, TSearchModel, TArgs>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, TArgs args, string? mappingName = null)
        where TEntity : class
        where TSearchModel : PaginationParams
        where TArgs : IServiceArgs;
    Task<PaginationModel<TModel>> Pagination<TEntity, TModel, TSearchModel>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, string? mappingName = null)
        where TEntity : class
        where TSearchModel : PaginationParams;
    Task<PaginationModel<TModel>> Pagination<TEntity, TModel, TSearchModel, TArgs>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, TArgs args, string? mappingName = null)
        where TEntity : class
        where TSearchModel : PaginationParams
        where TArgs : IServiceArgs;
    IQueryable<TModel> QueryOne<TEntity, TModel, TKey>(TKey id, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    IQueryable<TModel> QueryOne<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : IServiceArgs;
    IQueryable<TModel> QueryOne<TEntity, TModel>(int id, string? mappingName = null)
        where TEntity : class, IEntity;
    IQueryable<TModel> QueryOne<TEntity, TModel, TArgs>(int id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity
        where TArgs : IServiceArgs;
    TModel? Get<TEntity, TModel>(Expression<Func<TEntity, bool>> filter, string? mappingName = null)
        where TEntity : class;
    TModel? Get<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs;
    Task<TModel?> GetAsync<TEntity, TModel>(Expression<Func<TEntity, bool>> filter, string? mappingName = null)
        where TEntity : class;
    Task<TModel?> GetAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs;
    TModel? Get<TEntity, TModel, TKey>(TKey id, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    TModel? Get<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : IServiceArgs;
    Task<TModel?> GetAsync<TEntity, TModel, TKey>(TKey id, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task<TModel?> GetAsync<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : IServiceArgs;
    TModel? Get<TEntity, TModel>(int id, string? mappingName = null)
        where TEntity : class, IEntity;
    TModel? Get<TEntity, TModel, TArgs>(int id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity
        where TArgs : IServiceArgs;
    Task<TModel?> GetAsync<TEntity, TModel>(int id, string? mappingName = null)
        where TEntity : class, IEntity;
    Task<TModel?> GetAsync<TEntity, TModel, TArgs>(int id, TArgs args, string? mappingName = null)
        where TEntity : class, IEntity
        where TArgs : IServiceArgs;
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
    bool Any<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class;
    bool Any<TEntity, TArgs>(TArgs args)
        where TEntity : class;
    Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null)
        where TEntity : class;
    Task<bool> AnyAsync<TEntity, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args)
        where TEntity : class;
    Task<bool> AnyAsync<TEntity, TArgs>(TArgs args)
        where TEntity : class;
    TEntity Add<TModel, TEntity>(TModel model, string? addMappingName = null)
        where TEntity : class;
    TEntity Add<TModel, TEntity, TArgs>(TModel model, TArgs args, string? addMappingName = null)
        where TEntity : class;
    TResult? Add<TModel, TEntity, TResult, TKey>(TModel model, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    TResult? Add<TModel, TEntity, TResult>(TModel model, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity;
    Task<TEntity> AddAsync<TModel, TEntity>(TModel model, string? addMappingName = null)
        where TEntity : class;
    Task<TEntity> AddAsync<TModel, TEntity, TArgs>(TModel model, TArgs args, string? addMappingName = null)
        where TEntity : class;
    Task<TResult?> AddAsync<TModel, TEntity, TResult, TKey>(TModel model, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task<TResult?> AddAsync<TModel, TEntity, TResult>(TModel model, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity;
    IEnumerable<TEntity> AddRange<TModel, TEntity>(IEnumerable<TModel> models, string? addMappingName = null)
        where TEntity : class;
    IEnumerable<TEntity> AddRange<TModel, TEntity, TArgs>(IEnumerable<TModel> models, TArgs args, string? addMappingName = null)
        where TEntity : class;
    IEnumerable<TResult> AddRange<TModel, TEntity, TResult, TKey>(IEnumerable<TModel> models, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    IEnumerable<TResult> AddRange<TModel, TEntity, TResult>(IEnumerable<TModel> models, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity;
    Task<IEnumerable<TEntity>> AddRangeAsync<TModel, TEntity>(IEnumerable<TModel> models, string? addMappingName = null)
        where TEntity : class;
    Task<IEnumerable<TEntity>> AddRangeAsync<TModel, TEntity, TArgs>(IEnumerable<TModel> models, TArgs args, string? addMappingName = null)
        where TEntity : class;
    Task<IEnumerable<TResult>> AddRangeAsync<TModel, TEntity, TResult, TKey>(IEnumerable<TModel> models, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task<IEnumerable<TResult>> AddRangeAsync<TModel, TEntity, TResult>(IEnumerable<TModel> models, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity;
    TEntity? Update<TModel, TEntity, TKey>(TKey id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    TResult? Update<TModel, TEntity, TResult, TKey>(TKey id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    TResult? Update<TModel, TEntity, TResult>(int id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity
        where TResult : class;
    Task<TEntity?> UpdateAsync<TModel, TEntity, TKey>(TKey id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task<TResult?> UpdateAsync<TModel, TEntity, TResult, TKey>(TKey id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;
    Task<TResult?> UpdateAsync<TModel, TEntity, TResult>(int id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity
        where TResult : class;
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
    int Delete<TEntity>(int id)
        where TEntity : class, IEntity;
    int Delete<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class;
    Task<int> DeleteAsync<TEntity>(int id)
        where TEntity : class, IEntity;
    Task<int> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class;
    int DeleteRange<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class;
    Task<int> DeleteRangeAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
         where TEntity : class;
}
