using DevTeam.GenericRepository.Tests.Context.RentalContext;
using DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;
using DevTeam.Extensions.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTeam.GenericRepository.Tests.Context
{
    public class RelatedDataService : IRelatedDataService<PermissionsData>
    {
        private readonly IUserContext<Person> _userContext;

        public RelatedDataService(IUserContext<Person> userContext)
        {
            _userContext = userContext;
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
