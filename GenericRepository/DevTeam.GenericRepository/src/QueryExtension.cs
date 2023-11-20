using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;

namespace DevTeam.GenericRepository
{
    public abstract class QueryExtension<TEntity, TSettings> : IQueryExtension<TEntity,TSettings>
        where TSettings : QueryOptions
    {
        public Type Type => typeof(TEntity);
        public abstract Func<TSettings, bool> CanApply { get; }
        public abstract IQueryable<TEntity> Apply(IQueryable<TEntity> query);

        public IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query)
        {
            if (Type.IsAssignableFrom(typeof(TEntity)))
            {
                query = Apply(query);
            }            
            return query;
        }
    }
}
