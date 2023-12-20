using System.Collections.Generic;
using System;
using Microsoft.Extensions.DependencyInjection;
using DevTeam.Permissions.Core;

namespace DevTeam.Permissions;

public static class PermissionsValidators
{
    private static readonly List<Type> _validators = new();

    public static List<Type> Validators => _validators;

    public static IServiceCollection AddPermissionsValidator<TEntity, TRelatedData>(this IServiceCollection services)
        where TEntity : BasePermissionsValidator<TRelatedData>
    {
        _validators.Add(typeof(TEntity));
        services.AddScoped(typeof(TEntity));

        return services;
    }
}