using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class IsReadOnlyQueryExtension<TEntity, TOptions> : QueryExtension<TEntity, TOptions>
        where TEntity : class
        where TOptions : QueryOptions
    {
        public override int Order => 0;

        public override Func<TOptions, bool> CanApply => x => x.IsReadOnly;

        public override IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query) => query.AsNoTracking();
    }
}
