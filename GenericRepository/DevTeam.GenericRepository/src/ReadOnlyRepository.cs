using DevTeam.Extensions.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class ReadOnlyRepository : ReadOnlyRepository<IDbContext, QueryOptions>, IReadOnlyRepository
    {
        public ReadOnlyRepository(IDbContext context, IServiceProvider serviceProvider, QueryOptions? options = null)
            : base(context, serviceProvider, options)
        { }
    }

    public class ReadOnlyRepository<TContext, TOptions>: Repository<TContext, TOptions>, IReadOnlyRepository<TContext, TOptions>
        where TContext : IDbContext
        where TOptions : QueryOptions, new()
    {
        public ReadOnlyRepository(TContext context,  IServiceProvider serviceProvider, TOptions? options = null)
            :base(context, serviceProvider, options)
        {
            DefaultOptions.isReadOnly = true;
        }

        /*protected override IQueryable<TEntity> GetQuery<TEntity>(TOptions? options = null)
        {
            return Context.Set<TEntity>().AsNoTracking();
        }*/
    }
}
