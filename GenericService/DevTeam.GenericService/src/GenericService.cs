using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.GenericRepository;
using DevTeam.QueryMappings.Services.Interfaces;

namespace DevTeam.GenericService;

public class SoftDeleteGenericService : SoftDeleteGenericService<IDbContext>, ISoftDeleteGenericService
{
    public SoftDeleteGenericService(
        IMappingService<IDbContext> mappings,
        ISoftDeleteRepository<IDbContext> repository,
        IReadOnlyDeleteRepository<IDbContext> readRepository)
        : base(mappings, repository, readRepository)
    {
    }
}

public class SoftDeleteGenericService<TContext> : GenericService<TContext>, ISoftDeleteGenericService<TContext>
    where TContext : IDbContext
{
    public SoftDeleteGenericService(
        IMappingService<TContext> mappings,
        ISoftDeleteRepository<TContext> repository,
        IReadOnlyDeleteRepository<TContext> readRepository)
        : base(mappings, repository, readRepository)
    { }
}

public partial class GenericService : GenericService<IDbContext>, IGenericService
{
    public GenericService(
        IMappingService<IDbContext> mappings,
        IRepository repository,
        IReadOnlyRepository readRepository)
        : base(mappings, repository, readRepository)
    { }
}

public partial class GenericService<TContext> : IGenericService<TContext>
    where TContext : IDbContext
{
    private readonly IMappingService _mappings;
    private readonly IReadOnlyRepository<TContext> _readRepository;
    private readonly IRepository<TContext> _writeRepository;

    public GenericService(
        IMappingService<TContext> mappings,
        IRepository<TContext> repository,
        IReadOnlyRepository<TContext> readRepository)
    {
        _mappings = mappings;
        _readRepository = readRepository;
        _writeRepository = repository;
    }
}
