using DevTeam.Extensions.Abstractions;
using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericRepository;
using DevTeam.GenericService;
using DevTeam.GenericService.Pagination;
using DevTeam.QueryMappings.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevTeam.Permissions.Core;

public class SecureGenericService : SecureGenericService<IDbContext, QueryOptions>, ISecureGenericService
{
    public SecureGenericService(
        IMappingService<IDbContext> mappings,
        IRepository repository,
        IReadOnlyRepository<IDbContext, QueryOptions> readRepository,
        IPermissionsService permissionsService)
        : base(mappings, repository, readRepository, permissionsService)
    { }
}

public class SecureGenericService<TOptions> : SecureGenericService<IDbContext, TOptions>, ISecureGenericService<TOptions>
    where TOptions : QueryOptions, new()
{
    public SecureGenericService(
        IMappingService<IDbContext> mappings,
        IRepository<TOptions> repository,
        IReadOnlyRepository<TOptions> readRepository,
        IPermissionsService permissionsService)
        : base(mappings, repository, readRepository, permissionsService)
    { }
}

public class SecureGenericService<TContext, TOptions> : GenericService<TContext, TOptions>, ISecureGenericService<TContext, TOptions>
    where TContext : IDbContext
    where TOptions : QueryOptions, new()
{
    private readonly IPermissionsService _permissionsService;
    private readonly IMappingService<TContext> _mappingsService;

    public SecureGenericService(
        IMappingService<TContext> mappings,
        IRepository<TContext, TOptions> repository,
        IReadOnlyRepository<TContext, TOptions> readRepository,
        IPermissionsService permissionsService) : base(mappings, repository, readRepository)
    {
        _permissionsService = permissionsService;
        _mappingsService = mappings;
    }

    public virtual IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(TArgs args, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return QueryList<TEntity, TModel, TArgs>(args, mappingName, options);
    }

    public virtual IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null) 
        where TEntity : class
        where TArgs: class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return QueryList<TEntity, TModel, TArgs>(filter, args, mappingName, options);
    }

    public virtual List<TModel> GetList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null) 
        where TEntity : class 
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return GetList<TEntity, TModel, TArgs>(filter, args, mappingName, options);
    }

    public virtual Task<List<TModel>> GetListAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null) 
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return GetListAsync<TEntity, TModel, TArgs>(filter, args, mappingName, options);
    }

    public virtual Task<List<TModel>> Search<TEntity, TModel, TSearchModel, TArgs>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, TArgs args, TOptions? options = null) 
        where TEntity : class 
        where TSearchModel : PaginationParams
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return Search<TEntity, TModel, TSearchModel, TArgs>(searchService, searchModel, args, mappingName, options);
    }

    public virtual Task<PaginationModel<TModel>> Pagination<TEntity, TModel, TSearchModel, TArgs>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, TArgs args, TOptions? options = null)
        where TEntity : class
        where TSearchModel : PaginationParams
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return Pagination<TEntity, TModel, TSearchModel, TArgs>(searchService, searchModel, args, mappingName, options);
    }

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel, TArgs, TKey>(TKey id, TArgs args, TOptions? options = null) 
        where TEntity : class, IEntity<TKey>  
        where TKey : IEquatable<TKey>
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return QueryOne<TEntity, TModel, TKey, TArgs>(id, args, mappingName, options);
    }

    public virtual IQueryable<TModel> QueryOne<TEntity, TModel, TArgs>(int id, TArgs args, TOptions? options = null) 
        where TEntity : class, IEntity 
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return QueryOne<TEntity, TModel, TArgs, int>(id, args, options);
    }

    public virtual TModel Get<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null) 
        where TEntity : class 
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return Get<TEntity, TModel, TArgs>(filter, args, mappingName, options);
    }

    public virtual Task<TModel> GetAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, TOptions? options = null) 
        where TEntity : class 
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return GetAsync<TEntity, TModel, TArgs>(filter, args, mappingName, options);
    }

    public virtual TModel Get<TEntity, TModel, TArgs, TKey>(TKey id, TArgs args, TOptions? options = null) 
        where TEntity : class, IEntity<TKey> 
        where TArgs : class, IPermissionsArgs, IServiceArgs
        where TKey : IEquatable<TKey>
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return Get<TEntity, TModel, TKey, TArgs>(id, args, mappingName, options);
    }

    public virtual Task<TModel> GetAsync<TEntity, TModel, TKey, TArgs>(TKey id, TArgs args, TOptions? options = null) 
        where TEntity : class, IEntity<TKey> 
        where TArgs : class, IPermissionsArgs, IServiceArgs
        where TKey : IEquatable<TKey>
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return GetAsync<TEntity, TModel, TKey, TArgs>(id, args, mappingName, options);
    }

    public virtual TModel Get<TEntity, TModel, TArgs>(int id, TArgs args, TOptions? options = null) 
        where TEntity : class, IEntity 
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return Get<TEntity, TModel, TArgs>(id, args, mappingName, options);
    }

    public virtual Task<TModel> GetAsync<TEntity, TModel, TArgs>(int id, TArgs args, TOptions? options = null) 
        where TEntity : class, IEntity 
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var mappingName = GenerateMappingName(args.OtherPermissions);
        return GetAsync<TEntity, TModel, TArgs>(id, args, mappingName, options);
    }

    public virtual TResult Add<TModel, TEntity, TResult, TKey>(TModel model, int[] permissions, string? addMappingName = null) 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        var getMappingName = GenerateMappingName(permissions);
        return Add<TModel, TEntity, TResult, TKey>(model, addMappingName, getMappingName);
    }

    public virtual TResult Add<TModel, TEntity, TResult>(TModel model, int[] permissions, string? addMappingName = null)
        where TEntity : class, IEntity
    {
        return Add<TModel, TEntity, TResult, int>(model, permissions, addMappingName);
    }

    public virtual Task<TResult> AddAsync<TModel, TEntity, TResult, TKey>(TModel model, int[] permissions, string? addMappingName = null) 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        var getMappingName = GenerateMappingName(permissions);
        return AddAsync<TModel, TEntity, TResult, TKey>(model, addMappingName, getMappingName);
    }

    public virtual Task<TResult> AddAsync<TModel, TEntity, TResult>(TModel model, int[] permissions, string? addMappingName = null) 
        where TEntity : class, IEntity
    {
        return AddAsync<TModel, TEntity, TResult, int>(model, permissions, addMappingName);
    }

    public virtual List<TResult> AddRange<TModel, TEntity, TResult, TKey>(List<TModel> models, int[] permissions, string? addMappingName = null) 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        var getMappingName = GenerateMappingName(permissions);
        return AddRange<TModel, TEntity, TResult, TKey>(models, addMappingName, getMappingName);
    }

    public virtual List<TResult> AddRange<TModel, TEntity, TResult>(List<TModel> models, int[] permissions, string? addMappingName = null) 
        where TEntity : class, IEntity
    {
        return AddRange<TModel, TEntity, TResult, int>(models, permissions, addMappingName);
    }

    public virtual Task<List<TResult>> AddRangeAsync<TModel, TEntity, TResult, TKey>(List<TModel> models, int[] permissions, string? addMappingName = null) 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        var getMappingName = GenerateMappingName(permissions);
        return AddRangeAsync<TModel, TEntity, TResult, TKey>(models, addMappingName, getMappingName);
    }

    public virtual Task<List<TResult>> AddRangeAsync<TModel, TEntity, TResult>(List<TModel> models, int[] permissions, string? addMappingName = null) 
        where TEntity : class, IEntity
    {
        return AddRangeAsync<TModel, TEntity, TResult, int>(models, permissions, addMappingName);
    }

    public virtual TResult Update<TModel, TEntity, TResult, TKey>(TKey id, TModel model, int[] permissions, Action<TModel, TEntity> updateFunc) 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        var getMappingName = GenerateMappingName(permissions);
        Update(id, model, updateFunc);
        return Get<TEntity, TResult, TKey>(id, getMappingName);
    }

    public virtual TResult Update<TModel, TEntity, TResult>(int id, TModel model, int[] permissions, Action<TModel, TEntity> updateFunc) 
        where TEntity : class, IEntity
    {
        var getMappingName = GenerateMappingName(permissions);
        return Update<TModel, TEntity, TResult, int>(id, model, permissions, updateFunc);
    }

    public virtual async Task<TResult> UpdateAsync<TModel, TEntity, TResult, TKey>(TKey id, TModel model, int[] permissions, Action<TModel, TEntity> updateFunc) 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>
    {
        var getMappingName = GenerateMappingName(permissions);
        await UpdateAsync(id, model, updateFunc);
        return await GetAsync<TEntity, TResult, TKey>(id, getMappingName);
    }

    public virtual Task<TResult> UpdateAsync<TModel, TEntity, TResult>(int id, TModel model, int[] permissions, Action<TModel, TEntity> updateFunc) 
        where TEntity : class, IEntity
    {
        var getMappingName = GenerateMappingName(permissions);
        return UpdateAsync<TModel, TEntity, TResult, int>(id, model, permissions, updateFunc);
    }

    private string GenerateMappingName(params int[] permissions)
    {
        var allowed = _permissionsService.GetCurrentAccountPermissions();

        var intersection = permissions
            .OrderBy(x => x)
            .Where(x => allowed.Any(p => p.Id == x));

        return string.Join(string.Empty, intersection);
    }
}
