using DevTeam.Extensions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTeam.GenericService;

public partial class GenericService<TContext>
{
    #region Add

    public virtual TEntity Add<TModel, TEntity>(TModel model, string? addMappingName = null)
        where TEntity : class
    {
        var entity = _mappings.Map<TModel, TEntity>(model, addMappingName);

        _writeRepository.Add(entity);
        _writeRepository.Save();

        return entity;
    }

    public virtual TEntity Add<TModel, TEntity, TArgs>(TModel model, TArgs addMappingArgs, string? addMappingName = null)
        where TEntity : class
    {
        var entity = _mappings.Map<TModel, TEntity, TArgs>(model, addMappingArgs, addMappingName);

        _writeRepository.Add(entity);
        _writeRepository.Save();

        return entity;
    }

    public virtual TResult? Add<TModel, TEntity, TResult, TKey>(TModel model, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        var entity = Add<TModel, TEntity>(model, addMappingName);
        return Get<TEntity, TResult, TKey>(entity.Id, getMappingName);
    }

    public virtual TResult? Add<TModel, TEntity, TResult>(TModel model, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity
    {
        return Add<TModel, TEntity, TResult, int>(model, addMappingName, getMappingName);
    }

    public virtual async Task<TEntity> AddAsync<TModel, TEntity>(TModel model, string? addMappingName = null)
        where TEntity : class
    {
        var entity = _mappings.Map<TModel, TEntity>(model, addMappingName);

        await _writeRepository.AddAsync(entity);
        await _writeRepository.SaveAsync();

        return entity;
    }

    public virtual async Task<TEntity> AddAsync<TModel, TEntity, TArgs>(TModel model, TArgs args, string? addMappingName = null)
        where TEntity : class
    {
        var entity = _mappings.Map<TModel, TEntity, TArgs>(model, args, addMappingName);

        await _writeRepository.AddAsync(entity);
        await _writeRepository.SaveAsync();

        return entity;
    }

    public virtual async Task<TResult?> AddAsync<TModel, TEntity, TResult, TKey>(TModel model, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        var entity = await AddAsync<TModel, TEntity>(model, addMappingName);
        return await GetAsync<TEntity, TResult, TKey>(entity.Id, getMappingName);
    }

    public virtual Task<TResult?> AddAsync<TModel, TEntity, TResult>(TModel model, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity
    {
        return AddAsync<TModel, TEntity, TResult, int>(model, addMappingName, getMappingName);
    }

    #endregion

    #region AddRange

    public virtual IEnumerable<TEntity> AddRange<TModel, TEntity>(IEnumerable<TModel> models, string? addMappingName = null)
        where TEntity : class
    {
        var entities = _mappings.Map<TModel, TEntity>(models, addMappingName);

        _writeRepository.AddRange(entities);
        _writeRepository.Save();

        return entities;
    }

    public virtual IEnumerable<TEntity> AddRange<TModel, TEntity, TArgs>(IEnumerable<TModel> models, TArgs args, string? addMappingName = null)
        where TEntity : class
    {
        var entities = _mappings.Map<TModel, TEntity, TArgs>(models, args, addMappingName);

        _writeRepository.AddRange(entities);
        _writeRepository.Save();

        return entities;
    }

    public virtual IEnumerable<TResult> AddRange<TModel, TEntity, TResult, TKey>(IEnumerable<TModel> models, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        var entities = AddRange<TModel, TEntity>(models, addMappingName);

        var ids = entities.Select(x => x.Id).ToList();
        var results = GetList<TEntity, TResult>(x => ids.Contains(x.Id), getMappingName);

        return results;
    }

    public virtual IEnumerable<TResult> AddRange<TModel, TEntity, TResult>(IEnumerable<TModel> models, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity
    {
        return AddRange<TModel, TEntity, TResult, int>(models, addMappingName, getMappingName);
    }

    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync<TModel, TEntity>(IEnumerable<TModel> models, string? addMappingName = null)
        where TEntity : class
    {
        var entities = _mappings.Map<TModel, TEntity>(models, addMappingName);

        await _writeRepository.AddRangeAsync(entities);
        await _writeRepository.SaveAsync();

        return entities;
    }

    public virtual async Task<IEnumerable<TEntity>> AddRangeAsync<TModel, TEntity, TArgs>(IEnumerable<TModel> models, TArgs args, string? addMappingName = null)
        where TEntity : class
    {
        var entities = _mappings.Map<TModel, TEntity, TArgs>(models, args, addMappingName);

        await _writeRepository.AddRangeAsync(entities);
        await _writeRepository.SaveAsync();

        return entities;
    }

    public virtual async Task<IEnumerable<TResult>> AddRangeAsync<TModel, TEntity, TResult, TKey>(IEnumerable<TModel> models, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        var entities = await AddRangeAsync<TModel, TEntity>(models, addMappingName);

        var ids = entities.Select(x => x.Id).ToList();
        var results = await GetListAsync<TEntity, TResult>(x => ids.Contains(x.Id), getMappingName);

        return results;
    }

    public virtual Task<IEnumerable<TResult>> AddRangeAsync<TModel, TEntity, TResult>(IEnumerable<TModel> models, string? addMappingName = null, string? getMappingName = null)
        where TEntity : class, IEntity
    {
        return AddRangeAsync<TModel, TEntity, TResult, int>(models, addMappingName, getMappingName);
    }

    #endregion
}
