using DevTeam.GenericRepository.AspNetCore;
using DevTeam.QueryMappings.AspNetCore;
using DevTeam.QueryMappings.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DevTeam.GenericService.AspNetCore;

/// <summary>
/// Extensions for ASP.NET Core that help to add mappings in the system on application start. 
/// </summary>
public static class GenericServiceExtensions
{
    /// <summary>
    /// Adds all necessary services in provided IoC container.
    /// </summary>
    /// <param name="services">IServiceCollection instance that will be used to register services in IOC container</param>
    /// <returns>Returns provided IoC container with registred dependencies.</returns>
    public static IServiceCollection AddGenericServices(this IServiceCollection services)
    {
        services
            .AddQueryMappings()
            .AddGenericRepository()
            .AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>))
            .AddScoped<IGenericService, GenericService>()
            .AddScoped(typeof(ISoftDeleteGenericService<,>), typeof(SoftDeleteGenericService<,>))
            .AddScoped<ISoftDeleteGenericService, SoftDeleteGenericService>();

        return services;
    }

    /// <summary>
    /// Search for implementations of <see cref="IMappingsStorage"/> inside of list of assemblies and registers mappings for usage.
    /// </summary>
    /// <param name="app">IApplicationBuilder instance that will be used to initialize mappings</param>
    /// <param name="assemblies">List of assemblies where <see cref="IMappingsStorage"/> implementations are located.</param>
    /// <exception cref="MappingException">Thrown if we couldn't initialize mappings.</exception>
    public static void UseGenericServices(this IApplicationBuilder app, params Assembly[] assemblies)
    {
        app.UseQueryMappings(assemblies);
    }
}
