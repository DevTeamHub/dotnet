using DevTeam.GenericService;
using DevTeam.GenericService.Pagination;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using DevTeam.Extensions.Abstractions;
using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericRepository;

namespace DevTeam.Permissions.Core;

public interface ISecureGenericService<TOptions> : ISecureGenericService<IDbContext, TOptions>
    where TOptions : QueryOptions, new()
{ }

public interface ISecureGenericService : ISecureGenericService<IDbContext, QueryOptions>
{ }

public interface ISecureGenericService<TContext, TOptions> : IGenericService<TContext, TOptions>
    where TContext : IDbContext
    where TOptions : QueryOptions, new()
{
    IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    List<TModel> GetList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    Task<List<TModel>> GetListAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    Task<List<TModel>> Search<TEntity, TModel, TSearchModel, TArgs>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, TArgs args, TOptions? options = null)
        where TEntity : class
        where TSearchModel : PaginationParams
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    Task<PaginationModel<TModel>> Pagination<TEntity, TModel, TSearchModel, TArgs>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, TArgs args, TOptions? options = null)
        where TEntity : class
        where TSearchModel : PaginationParams
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    IQueryable<TModel> QueryOne<TEntity, TModel, TArgs, TKey>(TKey id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    IQueryable<TModel> QueryOne<TEntity, TModel, TArgs>(int id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    TModel Get<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    Task<TModel> GetAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    TModel Get<TEntity, TModel, TArgs, TKey>(TKey id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TArgs : class, IPermissionsArgs, IServiceArgs
        where TKey : IEquatable<TKey>;

    Task<TModel> GetAsync<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity<TKey>
        where TArgs : class, IPermissionsArgs, IServiceArgs
        where TKey : IEquatable<TKey>;

    TModel Get<TEntity, TModel, TArgs>(int id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    Task<TModel> GetAsync<TEntity, TModel, TArgs>(int id, TArgs args, TOptions? options = null)
        where TEntity : class, IEntity
        where TArgs : class, IPermissionsArgs, IServiceArgs;

    TResult Add<TModel, TEntity, TResult, TKey>(TModel model, int[] permissions, string? addMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;

    TResult Add<TModel, TEntity, TResult>(TModel model, int[] permissions, string? addMappingName = null)
        where TEntity : class, IEntity;

    Task<TResult> AddAsync<TModel, TEntity, TResult, TKey>(TModel model, int[] permissions, string? addMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;

    Task<TResult> AddAsync<TModel, TEntity, TResult>(TModel model, int[] permissions, string? addMappingName = null)
        where TEntity : class, IEntity;

    List<TResult> AddRange<TModel, TEntity, TResult, TKey>(List<TModel> models, int[] permissions, string? addMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;

    List<TResult> AddRange<TModel, TEntity, TResult>(List<TModel> models, int[] permissions, string? addMappingName = null)
        where TEntity : class, IEntity;

    Task<List<TResult>> AddRangeAsync<TModel, TEntity, TResult, TKey>(List<TModel> models, int[] permissions, string? addMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;

    Task<List<TResult>> AddRangeAsync<TModel, TEntity, TResult>(List<TModel> models, int[] permissions, string? addMappingName = null)
        where TEntity : class, IEntity;

    TResult Update<TModel, TEntity, TResult, TKey>(TKey id, TModel model, int[] permissions, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;

    TResult Update<TModel, TEntity, TResult>(int id, TModel model, int[] permissions, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity;

    Task<TResult> UpdateAsync<TModel, TEntity, TResult, TKey>(TKey id, TModel model, int[] permissions, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>;

    Task<TResult> UpdateAsync<TModel, TEntity, TResult>(int id, TModel model, int[] permissions, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity;
}
