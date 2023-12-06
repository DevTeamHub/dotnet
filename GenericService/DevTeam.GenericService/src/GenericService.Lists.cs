using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DevTeam.GenericRepository;

namespace DevTeam.GenericService;

public partial class GenericService<TContext, TOptions>
{
    public virtual IQueryable<TModel> QueryList<TEntity, TModel>(Expression<Func<TEntity, bool>>? filter = null, string? mappingName = null, TOptions? options = null)
        where TEntity : class
    {
        var query = _readRepository.GetList(filter, options);
        return _mappings.Map<TEntity, TModel>(query, mappingName);
    }

    public virtual IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var query = args.Type == ArgumentType.Mapping
            ? _readRepository.Query<TEntity>(options)
            : _readRepository.Query<TEntity, TArgs>(args, options);

        return args.Type == ArgumentType.Query
            ? _mappings.Map<TEntity, TModel>(query, mappingName)
            : _mappings.Map<TEntity, TModel, TArgs>(query, args, mappingName);
    }

    public virtual IQueryable<TModel> QueryList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        var query = args.Type == ArgumentType.Mapping
            ? _readRepository.GetList(filter, options)
            : _readRepository.GetList(filter, args, options);

        return args.Type == ArgumentType.Query
            ? _mappings.Map<TEntity, TModel>(query, mappingName)
            : _mappings.Map<TEntity, TModel, TArgs>(query, args, mappingName);
    }

    public virtual List<TModel> GetList<TEntity, TModel>(Expression<Func<TEntity, bool>>? filter = null, string? mappingName = null, TOptions? options = null)
        where TEntity : class
    {
        return QueryList<TEntity, TModel>(filter, mappingName, options).ToList();
    }

    public virtual List<TModel> GetList<TEntity, TModel, TArgs>(TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(args, mappingName, options).ToList();
    }

    public virtual List<TModel> GetList<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(filter, args, mappingName, options).ToList();
    }

    public virtual Task<List<TModel>> GetListAsync<TEntity, TModel>(Expression<Func<TEntity, bool>>? filter = null, string? mappingName = null, TOptions? options = null)
        where TEntity : class
    {
        return QueryList<TEntity, TModel>(filter, mappingName, options).ToListAsync();
    }

    public virtual Task<List<TModel>> GetListAsync<TEntity, TModel, TArgs>(TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(args, mappingName, options).ToListAsync();
    }

    public virtual Task<List<TModel>> GetListAsync<TEntity, TModel, TArgs>(Expression<Func<TEntity, bool>> filter, TArgs args, string? mappingName = null, TOptions? options = null)
        where TEntity : class
        where TArgs : class, IPermissionsArgs, IServiceArgs
    {
        return QueryList<TEntity, TModel, TArgs>(filter, args, mappingName, options).ToListAsync();
    }
}
