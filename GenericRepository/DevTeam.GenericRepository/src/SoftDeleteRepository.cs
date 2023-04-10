using DevTeam.Extensions.EntityFrameworkCore;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class SoftDeleteRepository : SoftDeleteRepository<IDbContext>, ISoftDeleteRepository
    {
        public SoftDeleteRepository(IDbContext context)
            : base(context)
        { }
    }

    public class SoftDeleteRepository<TContext>: Repository<TContext>, ISoftDeleteRepository<TContext>
        where TContext: IDbContext
    {
        public SoftDeleteRepository(TContext context)
            : base(context)
        { }

        public override IQueryable<TEntity> Query<TEntity>()
        {
            return GetQuery<TEntity>();
        }
    }
}
