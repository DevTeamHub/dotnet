using DevTeam.Extensions.Abstractions;
using DevTeam.GenericRepository.Tests.Context.RentalContext;
using DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;
using DevTeam.GenericService;
using DevTeam.Permissions.Core;

namespace DevTeam.GenericRepository.Tests.Context
{
    public class RelatedDataService : IRelatedDataService<PermissionsData>
    {
        private readonly IUserContext<Person> _userContext;
        private readonly IGenericService _service;

        public RelatedDataService(
            IUserContext<Person> userContext,
            IGenericService service
            )
        {
            _service = service;
            _userContext = userContext;
        }

        public Task<List<PermissionsData>> GetRelatedData<TEntity>(List<int> requestIds)
            where TEntity : class, IEntity
        {
            return _service.GetListAsync<TEntity, PermissionsData>(x => requestIds.Contains(x.Id));
        }


        public PermissionsData GetCurrentAccountRelatedData()
        {
            var user = _userContext.User!;

            return new PermissionsData
            {
                EntityId = user.Id,
                ApartmentId = user.ApartmentId,
            };
        }
    }
}
