using System;
using System.Linq;

namespace DevTeam.GenericRepository;

public interface IQueryExtension<TEntity, TOptions> 
    where TOptions : QueryOptions
{
    public int Order { get; }
    public Func<TOptions, bool> CanApply { get; }
    public IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query);
    public IQueryable<TEntity> ApplyExtension<TArgs>(IQueryable<TEntity> query, TArgs args);
}