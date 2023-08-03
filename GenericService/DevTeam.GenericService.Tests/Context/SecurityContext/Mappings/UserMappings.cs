using DevTeam.GenericService.Tests.Context.RentalContext.Models;
using DevTeam.GenericService.Tests.Context.SecurityContext.Entities;
using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;

namespace DevTeam.GenericService.Tests.Context.SecurityContext.Mappings;

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
