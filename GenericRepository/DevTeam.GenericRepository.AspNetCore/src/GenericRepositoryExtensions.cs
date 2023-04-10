using Microsoft.Extensions.DependencyInjection;

namespace DevTeam.GenericRepository.AspNetCore;

/// <summary>
/// Extensions for ASP.NET Core that help to add repositories in the system on application start. 
/// </summary>
public static class GenericRepositoryExtensions
{
    /// <summary>
    /// Adds all necessary services in provided IoC container.
    /// </summary>
    /// <param name="services">IServiceCollection instance that will be used to register services in IOC container</param>
    /// <returns>Returns provided IoC container with registred dependencies.</returns>
    public static IServiceCollection AddGenericRepository(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepository<>), typeof(Repository<>))
            .AddScoped<IRepository, Repository>()
            .AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>))
            .AddScoped<IReadOnlyRepository, ReadOnlyRepository>()
            .AddScoped(typeof(ISoftDeleteRepository<>), typeof(SoftDeleteRepository<>))
            .AddScoped<ISoftDeleteRepository, SoftDeleteRepository>()
            .AddScoped(typeof(IReadOnlyDeleteRepository<>), typeof(ReadOnlyDeleteRepository<>))
            .AddScoped<IReadOnlyDeleteRepository, ReadOnlyDeleteRepository>();

        return services;
    }
}
