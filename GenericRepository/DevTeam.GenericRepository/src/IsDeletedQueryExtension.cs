using DevTeam.Extensions.Abstractions;
using System;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class IsDeletedQueryExtension<TEntity> : QueryExtension<TEntity, QueryOptions> 
        where TEntity : IDeleted
    {
        public override Func<QueryOptions, bool> CanApply => x => !x.isDeleted;

        public override IQueryable<TEntity> Apply(IQueryable<TEntity> query) => query.Where(x => !x.IsDeleted);
    }
}
