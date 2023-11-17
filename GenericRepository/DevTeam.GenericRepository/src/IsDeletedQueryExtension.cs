using DevTeam.Extensions.Abstractions;
using System;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class IsDeletedQueryExtension : QueryExtension<IDeleted, QueryOptions>
    {
        public override Func<QueryOptions, bool> CanApply => x => x.isDeleted;

        public override IQueryable<IDeleted> Apply(IQueryable<IDeleted> query) => query.Where(x => x.IsDeleted);
    }
}
