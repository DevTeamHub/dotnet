using DevTeam.Extensions.EntityFrameworkCore;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class ReadOnlyDeleteRepository : ReadOnlyDeleteRepository<IDbContext>, IReadOnlyDeleteRepository
    {
        public ReadOnlyDeleteRepository(IDbContext context)
            : base(context)
        { }
    }

    public class ReadOnlyDeleteRepository<TContext>: ReadOnlyRepository<TContext>, IReadOnlyDeleteRepository<TContext>
        where TContext: IDbContext
    {
        public ReadOnlyDeleteRepository(TContext context)
            : base(context)
        { }

        public override IQueryable<TEntity> Query<TEntity>()
        {
            return GetQuery<TEntity>();
        }
    }
}
