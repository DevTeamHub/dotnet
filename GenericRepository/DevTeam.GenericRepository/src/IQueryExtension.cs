using System;
using System.Linq;

namespace DevTeam.GenericRepository;

public interface IQueryExtension<TEntity, TSettings> 
    where TSettings : QueryOptions
{
    public Func<TSettings, bool> CanApply { get; }
    public IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query);
}
