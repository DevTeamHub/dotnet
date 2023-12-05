using DevTeam.Extensions.Abstractions;
using System;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class IncludeDeletedQueryExtension<TEntity, TOptions> : QueryExtension<TEntity, TOptions> 
        where TEntity : IDeleted
        where TOptions : QueryOptions
    {
        public override Func<TOptions, bool> CanApply => x => !x.IncludeDeleted;

        public override IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query) => query.Where(x => !x.IsDeleted);
    }
}
