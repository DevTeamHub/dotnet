using System;
using System.Linq;

namespace DevTeam.GenericRepository;

public interface IQueryExtensionBase<TEntity, TOptions>
    where TOptions : QueryOptions
{
    public int Order { get; }
    public Func<TOptions, bool> CanApply { get; }
}

public interface IQueryExtension<TEntity, TOptions> : IQueryExtensionBase<TEntity, TOptions>
    where TOptions : QueryOptions
{
    public IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query);
}

public interface ISecurityQueryExtension<TEntity, TOptions> : IQueryExtensionBase<TEntity, TOptions>
    where TOptions : QueryOptions
{
    public IQueryable<TEntity> ApplyExtension<TArgs>(IQueryable<TEntity> query, TArgs args)
        where TArgs : class, IPermissionsArgs, IServiceArgs;
}
