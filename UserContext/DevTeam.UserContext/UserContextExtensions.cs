using DevTeam.Extensions.Abstractions;
using DevTeam.UserContext.Asbtractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevTeam.UserContext;

public static class UserContextExtensions
{
    public static IServiceCollection AddUserContext<TKey>(this IServiceCollection services)
        where TKey : struct
    {
        return services
            .AddScoped<IUserContext<TKey>, UserContext<TKey>>()
            .AddScoped<UserContextServiceFilter<TKey>>();
    }

    public static IServiceCollection AddUserContext<TUser, TUserModel, TKey>(this IServiceCollection services)
        where TUser : class, IEntity<TKey>
        where TUserModel : class
        where TKey: struct, IEquatable<TKey>
    {
        return services
            .AddUserContext<TKey>()
            .AddScoped<IUserContext<TUserModel, TKey>, UserContext<TUserModel, TKey>>()
            .AddScoped<UserContextServiceFilter<TUser, TUserModel, TKey>>();
    }

    public static void AddUserContext<TKey>(this MvcOptions options)
        where TKey : struct
    {
        options.Filters.Add<UserContextServiceFilter<TKey>>();
    }

    public static void AddUserContext<TUser, TUserModel, TKey>(this MvcOptions options)
        where TUser : class, IEntity<TKey>
        where TUserModel : class
        where TKey : struct, IEquatable<TKey>
    {
        options.Filters.Add<UserContextServiceFilter<TUser, TUserModel, TKey>>();
    }
}

