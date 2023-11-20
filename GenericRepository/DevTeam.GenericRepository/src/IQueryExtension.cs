using System;
using System.Linq;

namespace DevTeam.GenericRepository;

public interface IQueryExtension<TEntity, TSettings> 
    where TSettings : QueryOptions
{
    public Type Type { get; }
    public Func<TSettings, bool> CanApply { get; }
    public IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query);
}
