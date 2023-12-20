using System;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public abstract class QueryExtension<TEntity, TOptions> : IQueryExtension<TEntity, TOptions>
        where TOptions : QueryOptions
    {
        public virtual int Order => 1;
        public abstract Func<TOptions, bool> CanApply { get; }
        public abstract IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query);
        public abstract IQueryable<TEntity> ApplyExtension<TArgs>(IQueryable<TEntity> query, TArgs args);
    }
}
