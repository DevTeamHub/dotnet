using DevTeam.Extensions.Abstractions;
using DevTeam.GenericService;
using DevTeam.Session.Abstractions;
using DevTeam.UserContext.Asbtractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DevTeam.UserContext;

public static class UserContextExtensions
{
    public static IMvcBuilder AddUserContext(this IMvcBuilder builder)
    {
        return builder.AddUserContext<int>();
    }

    public static IMvcBuilder AddUserContext<TKey>(this IMvcBuilder builder)
        where TKey : struct
    {
        builder.Services
            .AddScoped<IUserContext<TKey>, UserContext<TKey>>()
            .AddScoped<IUserService<TKey>>((provider) => new UserService<TKey>())
            .AddScoped<UserContextServiceFilter<TKey>>()
            .AddHttpContextAccessor();

        builder.Services.Configure<MvcOptions>(options => {
            options.Filters.Add<UserContextServiceFilter<TKey>>();
        });

        return builder;
    }

    public static IMvcBuilder AddUserMapping<TUser, TUserModel>(this IMvcBuilder builder, UserContextOptions? options = null)
        where TUser : class, IEntity
        where TUserModel : class
    {
        return builder.AddUserMapping<TUser, TUserModel, int>(options);
    }

    public static IMvcBuilder AddUserMapping<TUser, TUserModel, TKey>(this IMvcBuilder builder, UserContextOptions? options = null)
        where TUser : class, IEntity<TKey>
        where TUserModel : class
        where TKey: struct, IEquatable<TKey>
    {
        options ??= new UserContextOptions();

        builder.Services
            .AddScoped<IUserContext<TKey>, UserContext<TUserModel, TKey>>()
            .AddScoped<IUserContext<TUserModel, TKey>, UserContext<TUserModel, TKey>>()
            .AddScoped<IUserService<TKey>>((provider) =>
                new UserService<TUser, TUserModel, TKey>(
                    options.UseSession ? provider.GetRequiredService<ISessionService>() : null,
                    provider.GetRequiredService<IGenericService>(),
                    provider.GetRequiredService<IUserContext<TUserModel, TKey>>()
                )
            );

        return builder;
    }
}

