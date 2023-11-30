using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class IsReadOnlyQueryExtension<TEntity, TOptions> : QueryExtension<TEntity, TOptions>
        where TEntity : class
        where TOptions : QueryOptions
    {
        public override Func<TOptions, bool> CanApply => x => x.isReadOnly;

        public override IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query) => query.AsNoTracking();
    }
}
