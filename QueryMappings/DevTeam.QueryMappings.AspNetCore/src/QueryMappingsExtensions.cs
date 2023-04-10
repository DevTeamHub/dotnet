using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;
using DevTeam.QueryMappings.Services.Implementations;
using DevTeam.QueryMappings.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace DevTeam.QueryMappings.AspNetCore
{
    /// <summary>
    /// Extensions for ASP.NET Core that help to add mappings in the system on application start. 
    /// </summary>
    public static class QueryMappingsExtensions
    {
        /// <summary>
        /// Adds all necessary services in provided IoC container.
        /// </summary>
        /// <param name="services">IServiceCollection instance that will be used to register services in IOC container</param>
        /// <returns>Returns provided IoC container with registred dependencies.</returns>
        public static IServiceCollection AddQueryMappings(this IServiceCollection services)
        {
            services
                .AddSingleton<IMappingsList, MappingsList>()
                .AddScoped<IMappingService, MappingService>()
                .AddScoped(typeof(IMappingService<>), typeof(MappingService<>));

            return services;
        }

        /// <summary>
        /// Search for implementations of <see cref="IMappingsStorage"/> inside of list of assemblies and registers mappings for usage.
        /// </summary>
        /// <param name="app">IApplicationBuilder instance that will be used to initialize mappings</param>
        /// <param name="assemblies">List of assemblies where <see cref="IMappingsStorage"/> implementations are located.</param>
        /// <exception cref="MappingException">Thrown if we couldn't initialize mappings.</exception>
        public static void UseQueryMappings(this IApplicationBuilder app, params Assembly[] assemblies)
        {
            var mappings = app.ApplicationServices.GetRequiredService<IMappingsList>();

            MappingsConfiguration.Register(mappings, assemblies);
        }
    }
}
