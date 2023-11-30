using System;
using System.Linq;

namespace DevTeam.GenericRepository;

public interface IQueryExtension<TEntity, TOptions> 
    where TOptions : QueryOptions
{
    public int Order { get; }
    public Type Type { get; }
    public Func<TOptions, bool> CanApply { get; }
    public IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query);
}
