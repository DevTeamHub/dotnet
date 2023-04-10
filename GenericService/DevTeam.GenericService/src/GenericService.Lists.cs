using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DevTeam.GenericService;

public partial class GenericService<TContext>
{
    public virtual IQueryable<TModel> QueryList<TEntity, TModel>(Expression<Func<TEntity, bool>>? filter = null, string? mappingName = null)
        where TEntity : class
    {
        var query = _readRepository.GetList(filter);
        return _mappings.Map<TEntity, TModel>(query, mappingName);
    }

    public virtual IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs
    {
        var query = args.Type == ArgumentType.Mapping
            ? _readRepository.Query<TEntity>()
            : _readRepository.Query<TEntity, TArgs>(args);

        return args.Type == ArgumentType.Query
            ? _mappings.Map<TEntity, TModel>(query, mappingName)
            : _mappings.Map<TEntity, TModel, TArgs>(query, args, mappingName);
    }

    public virtual IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs
    {
        var query = args.Type == ArgumentType.Mapping
            ? _readRepository.GetList(filter)
            : _readRepository.GetList(filter, args);

        return args.Type == ArgumentType.Query
            ? _mappings.Map<TEntity, TModel>(query, mappingName)
            : _mappings.Map<TEntity, TModel, TArgs>(query, args, mappingName);
    }

    public virtual List<TModel> GetList<TEntity, TModel>(Expression<Func<TEntity, bool>>? filter = null, string? mappingName = null)
        where TEntity : class
    {
        return QueryList<TEntity, TModel>(filter, mappingName).ToList();
    }

    public virtual List<TModel> GetList<TEntity, TModel, TArgs>(TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(args, mappingName).ToList();
    }

    public virtual List<TModel> GetList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(filter, args, mappingName).ToList();
    }

    public virtual Task<List<TModel>> GetListAsync<TEntity, TModel>(Expression<Func<TEntity, bool>>? filter = null, string? mappingName = null)
        where TEntity : class
    {
        return QueryList<TEntity, TModel>(filter, mappingName).ToListAsync();
    }

    public virtual Task<List<TModel>> GetListAsync<TEntity, TModel, TArgs>(TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(args, mappingName).ToListAsync();
    }

    public virtual Task<List<TModel>> GetListAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null)
        where TEntity : class
        where TArgs : IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(filter, args, mappingName).ToListAsync();
    }
}
