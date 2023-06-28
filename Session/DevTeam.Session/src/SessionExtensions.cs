using DevTeam.Session;
using DevTeam.Session.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevTeam.Session;

public static class SessionExtensions
{
    public static IServiceCollection AddSession(this IServiceCollection services, TimeSpan idleTimeout)
    {
        services
            .AddScoped<ISessionService, SessionService>()
            .AddDistributedMemoryCache()
            .AddSession(options =>
            {
                options.IdleTimeout = idleTimeout;
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

        return services;
    }

    public static IApplicationBuilder UseSessionConfiguration(this IApplicationBuilder app)
    {
        app.UseSession();

        return app;
    }
}
