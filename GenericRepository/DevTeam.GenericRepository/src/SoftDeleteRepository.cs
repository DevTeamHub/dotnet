using DevTeam.Extensions.EntityFrameworkCore;
using System;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class SoftDeleteRepository : SoftDeleteRepository<IDbContext, QueryOptions>, ISoftDeleteRepository
    {
        public SoftDeleteRepository(IDbContext context, IServiceProvider serviceProvider, QueryOptions? options = null)
            : base(context, serviceProvider, options)
        { }
    }

    public class SoftDeleteRepository<TOptions> : SoftDeleteRepository<IDbContext, TOptions>, ISoftDeleteRepository<TOptions>
        where TOptions : QueryOptions, new()
    {
        public SoftDeleteRepository(IDbContext context, IServiceProvider serviceProvider, TOptions? options = null)
            : base(context, serviceProvider, options)
        { }
    }

    public class SoftDeleteRepository<TContext, TOptions> : Repository<TContext, TOptions>, ISoftDeleteRepository<TContext, TOptions>
        where TContext : IDbContext
        where TOptions : QueryOptions, new()
    {
        public SoftDeleteRepository(TContext context, IServiceProvider serviceProvider, TOptions? options = null)
            : base(context, serviceProvider, options)
        {
            DefaultOptions.IncludeDeleted = true;
        }
    }
}
