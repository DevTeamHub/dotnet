using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevTeam.GenericRepository
{
    public abstract class SecurityQueryExtension<TEntity, TRelatedData, TScopes, TOptions> : ISecurityQueryExtension<TEntity, TOptions>
    where TRelatedData : RelatedData
    where TScopes : struct, Enum
    where TOptions : QueryOptions
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

        public virtual int Order => 3;
        public Func<TOptions, bool> CanApply => x => x.ApplySecurity;

       public IQueryable<TEntity> ApplyExtension<TArgs>(IQueryable<TEntity> query, TArgs args)
            where TArgs : class, IPermissionsArgs, IServiceArgs
        {
            var permission = PermissionsService
                .GetCurrentAccountPermissions()
                .First(x => x.Id == args.AccessPermission);

            var scopes = permission.Scopes.HasValue ? (TScopes?)Enum.ToObject(typeof(TScopes), permission.Scopes.Value) : null;

            if (!scopes.HasValue) return query;

            var relatedData = RelatedDataService.GetCurrentAccountRelatedData();

            query = Filter(query, relatedData, scopes.Value);
            return query;
        }

        public abstract IQueryable<TEntity> Filter(IQueryable<TEntity> query, TRelatedData relatedData, TScopes scopes);
    }

}
