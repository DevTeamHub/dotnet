using DevTeam.GenericRepository;
using System;
using System.Linq;

namespace DevTeam.Permissions.Core;

public abstract class SecurityQueryExtension<TEntity, TRelatedData, TScopes, TOptions> : QueryExtension<TEntity, TOptions>
    where TRelatedData : RelatedData
    where TScopes : struct, Enum
    where TOptions : QueryOptions, ISecurityOptions
{
    protected readonly IPermissionsService PermissionsService;
    protected readonly IRelatedDataService<TRelatedData> RelatedDataService;

    public SecurityQueryExtension(
        IPermissionsService permissionsService,
        IRelatedDataService<TRelatedData> relatedDataService)
    {
        PermissionsService = permissionsService;
        RelatedDataService = relatedDataService;
    }

    public override int Order => 3;
    public override Func<TOptions, bool> CanApply => x => x.ApplySecurity;
    public override IQueryable<TEntity> ApplyExtension(IQueryable<TEntity> query)
    {
        return query;
    }

   public override IQueryable<TEntity> ApplyExtension<TArgs>(IQueryable<TEntity> query, TArgs args)
   {
        if (!typeof(IPermissionsArgs).IsAssignableFrom(typeof(TArgs)) || args == null)
        {
            return ApplyExtension(query);
        }

        var permissionArgs = (IPermissionsArgs)args;
        var permission = PermissionsService
            .GetCurrentAccountPermissions()
            .First(x => x.Id == permissionArgs.AccessPermission);

        var scopes = permission.Scopes.HasValue ? (TScopes?)Enum.ToObject(typeof(TScopes), permission.Scopes.Value) : null;

        if (!scopes.HasValue) return query;

        var relatedData = RelatedDataService.GetCurrentAccountRelatedData();

        query = Filter(query, relatedData, scopes.Value);
        return query;
   }

    public abstract IQueryable<TEntity> Filter(IQueryable<TEntity> query, TRelatedData relatedData, TScopes scopes);
}
