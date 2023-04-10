using DevTeam.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class ReadOnlyRepository : ReadOnlyRepository<IDbContext>, IReadOnlyRepository
    {
        public ReadOnlyRepository(IDbContext context)
            : base(context)
        { }
    }

    public class ReadOnlyRepository<TContext>: Repository<TContext>, IReadOnlyRepository<TContext>
        where TContext : IDbContext
    {
        public ReadOnlyRepository(TContext context)
            :base(context)
        { }

        protected override IQueryable<TEntity> GetQuery<TEntity>()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }
    }
}
