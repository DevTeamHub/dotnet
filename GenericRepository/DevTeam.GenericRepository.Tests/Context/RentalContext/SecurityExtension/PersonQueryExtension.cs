using DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext
{
    public class PersonQueryExtension : SecurityQueryExtension<Person, PermissionsData, PermissionScopes, TestQueryOptions>
    {
        public PersonQueryExtension(
            IPermissionsService permissionsService,
            IRelatedDataService<PermissionsData> relatedDataService)
            : base(permissionsService, relatedDataService)
        { }

        public override IQueryable<Person> Filter(IQueryable<Person> query, PermissionsData relatedData, PermissionScopes scopes)
        {
            if (scopes.HasFlag(PermissionScopes.Apartments))
            {
                query = query.Where(x => x.ApartmentId == relatedData.ApartmentId);
            }

            return query;
        }
    }
}
