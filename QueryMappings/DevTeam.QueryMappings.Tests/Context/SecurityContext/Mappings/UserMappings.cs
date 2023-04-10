using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;
using DevTeam.QueryMappings.Tests.Context.RentalContext.Models;
using DevTeam.QueryMappings.Tests.Context.SecurityContext.Entities;

namespace DevTeam.QueryMappings.Tests.Context.SecurityContext.Mappings
{
    public class UserMappings: IMappingsStorage
    {
        public void Setup(IMappingsList mappings)
        {
            mappings.Add<User, UserModel>(x => new UserModel
            {
                Id = x.Id,
                UserName = x.UserName,
                Password = x.Password,
                IsAdmin = x.IsAdmin
            });
        }
    }
}
