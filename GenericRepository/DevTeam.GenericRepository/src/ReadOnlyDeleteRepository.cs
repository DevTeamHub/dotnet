using DevTeam.Extensions.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DevTeam.GenericRepository
{
    public class ReadOnlyDeleteRepository : ReadOnlyDeleteRepository<IDbContext, QueryOptions>, IReadOnlyDeleteRepository
    {
        public ReadOnlyDeleteRepository(IDbContext context, IServiceProvider serviceProvider, QueryOptions? options = null)
            : base(context, serviceProvider, options)
        { }
    }

    public class ReadOnlyDeleteRepository<TOptions> : ReadOnlyDeleteRepository<IDbContext, TOptions>, IReadOnlyDeleteRepository<TOptions>
        where TOptions : QueryOptions, new()
    {
        public ReadOnlyDeleteRepository(IDbContext context, IServiceProvider serviceProvider, TOptions? options = null)
            : base(context, serviceProvider, options)
        { }
    }

    public class ReadOnlyDeleteRepository<TContext, TOptions> : ReadOnlyRepository<TContext, TOptions>, IReadOnlyDeleteRepository<TContext, TOptions>
        where TContext : IDbContext
        where TOptions : QueryOptions, new()
    {
        public ReadOnlyDeleteRepository(TContext context, IServiceProvider serviceProvider, TOptions? options = null)
            : base(context, serviceProvider, options)
        {
            DefaultOptions.isDeleted = true;
        }

        public override IQueryable<TEntity> Query<TEntity>(TOptions? options = null)
        {
            return GetQuery<TEntity>();
        }
    }
}
