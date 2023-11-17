using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericRepository;
using DevTeam.QueryMappings.Services.Interfaces;
using System.Diagnostics;

namespace DevTeam.GenericService;

public class SoftDeleteGenericService : SoftDeleteGenericService<IDbContext, QueryOptions>, ISoftDeleteGenericService
{
    public SoftDeleteGenericService(
        IMappingService<IDbContext> mappings,
        ISoftDeleteRepository<IDbContext, QueryOptions> repository,
        IReadOnlyDeleteRepository<IDbContext, QueryOptions> readRepository)
        : base(mappings, repository, readRepository)
    {
    }
}

public class SoftDeleteGenericService<TContext, TOptions> : GenericService<TContext, TOptions>, ISoftDeleteGenericService<TContext, TOptions>
    where TContext : IDbContext
    where TOptions : QueryOptions
{
    public SoftDeleteGenericService(
        IMappingService<TContext> mappings,
        ISoftDeleteRepository<TContext, TOptions> repository,
        IReadOnlyDeleteRepository<TContext, TOptions> readRepository)
        : base(mappings, repository, readRepository)
    { }
}

public partial class GenericService : GenericService<IDbContext, QueryOptions>, IGenericService
{
    public GenericService(
        IMappingService<IDbContext> mappings,
        IRepository repository,
        IReadOnlyRepository readRepository)
        : base(mappings, repository, readRepository)
    { }
}

public partial class GenericService<TContext, TOptions> : IGenericService<TContext, TOptions>
    where TContext : IDbContext
    where TOptions : QueryOptions
{
    private readonly IMappingService _mappings;
    private readonly IReadOnlyRepository<TContext, TOptions> _readRepository;
    private readonly IRepository<TContext, TOptions> _writeRepository;

    public GenericService(
        IMappingService<TContext> mappings,
        IRepository<TContext, TOptions> repository,
        IReadOnlyRepository<TContext, TOptions> readRepository)
    {
        _mappings = mappings;
        _readRepository = readRepository;
        _writeRepository = repository;
    }
}
