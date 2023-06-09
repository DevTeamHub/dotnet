﻿using DevTeam.GenericService.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTeam.GenericService;

public partial class GenericService<TContext>
{
    public virtual Task<List<TModel>> Search<TEntity, TModel, TSearchModel>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, string? mappingName = null)
    where TEntity : class
    where TSearchModel : PaginationParams
    {
        var query = GetBaseQuery<TEntity>();
        query = ApplyFilter(query, searchService, searchModel);
        query = ApplyPagination(query, searchModel);
        return _mappings.Map<TEntity, TModel>(query, mappingName).ToListAsync();
    }

    public virtual Task<List<TModel>> Search<TEntity, TModel, TSearchModel, TArgs>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, TArgs args, string? mappingName = null)
        where TEntity : class
        where TSearchModel : PaginationParams
        where TArgs : IServiceArgs
    {
        var query = GetBaseQuery<TEntity, TArgs>(args);
        query = ApplyFilter(query, searchService, searchModel);
        query = ApplyPagination(query, searchModel);
        return _mappings.Map<TEntity, TModel, TArgs>(query, args, mappingName).ToListAsync();
    }

    public virtual async Task<PaginationModel<TModel>> Pagination<TEntity, TModel, TSearchModel>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, string? mappingName = null)
        where TEntity : class
        where TSearchModel : PaginationParams
    {
        var query = GetBaseQuery<TEntity>();
        query = ApplyFilter(query, searchService, searchModel);

        var totalCount = await query.CountAsync();

        query = ApplyPagination(query, searchModel);

        var models = await _mappings.Map<TEntity, TModel>(query, mappingName).ToListAsync();

        return new PaginationModel<TModel>(models, totalCount);
    }

    public virtual async Task<PaginationModel<TModel>> Pagination<TEntity, TModel, TSearchModel, TArgs>(ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel, TArgs args, string? mappingName = null)
        where TEntity : class
        where TSearchModel : PaginationParams
        where TArgs : IServiceArgs
    {
        var query = GetBaseQuery<TEntity, TArgs>(args);
        query = ApplyFilter(query, searchService, searchModel);

        var totalCount = await query.CountAsync();

        query = ApplyPagination(query, searchModel);

        var mapped = args.Type == ArgumentType.Query
            ? _mappings.Map<TEntity, TModel>(query, mappingName)
            : _mappings.Map<TEntity, TModel, TArgs>(query, args, mappingName);

        var models = await mapped.ToListAsync();

        return new PaginationModel<TModel>(models, totalCount);
    }

    private IQueryable<TEntity> GetBaseQuery<TEntity>()
        where TEntity : class
    {
        return _readRepository.Query<TEntity>(); 
    }

    private IQueryable<TEntity> GetBaseQuery<TEntity, TArgs>(TArgs args)
        where TEntity : class
        where TArgs : IServiceArgs
    {
        return args.Type == ArgumentType.Mapping
            ? _readRepository.Query<TEntity>()
            : _readRepository.Query<TEntity, TArgs>(args);
    }

    private IQueryable<TEntity> ApplyFilter<TEntity, TSearchModel>(IQueryable<TEntity> query, ISearchService<TEntity, TSearchModel> searchService, TSearchModel searchModel)
        where TEntity : class
        where TSearchModel : PaginationParams
    {
        searchModel.Skip ??= 0;
        searchModel.Take ??= 20;

        query = searchService.Filter(query, searchModel);

        return query;
    }

    private IQueryable<TEntity> ApplyPagination<TEntity, TSearchModel>(IQueryable<TEntity> query, TSearchModel searchModel)
        where TEntity : class
        where TSearchModel : PaginationParams
    {
        return query.Skip(searchModel.Skip!.Value).Take(searchModel.Take!.Value);
    }
}
