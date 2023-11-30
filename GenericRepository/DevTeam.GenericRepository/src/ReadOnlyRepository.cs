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

    public class ReadOnlyRepository<TOptions> : ReadOnlyRepository<IDbContext, TOptions>, IReadOnlyRepository<TOptions>
        where TOptions : QueryOptions, new()
    {
        public ReadOnlyRepository(IDbContext context, IServiceProvider serviceProvider, TOptions? options = null)
            : base(context, serviceProvider, options)
        { }
    }

    public class ReadOnlyRepository<TContext, TOptions> : Repository<TContext, TOptions>, IReadOnlyRepository<TContext, TOptions>
        where TContext : IDbContext
        where TOptions : QueryOptions, new()
    {
        public ReadOnlyRepository(TContext context, IServiceProvider serviceProvider, TOptions? options = null)
            : base(context, serviceProvider, options)
        {
            DefaultOptions.IsReadOnly = true;
        }
    }
}
