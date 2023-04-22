using DevTeam.QueryMappings.Base;
using DevTeam.QueryMappings.Helpers;
using DevTeam.UserContext.Tests.Context.Entities;
using DevTeam.UserContext.Tests.Context.Models;

namespace DevTeam.UserContext.Tests.Context.Mappings;

public class UserMappings : IMappingsStorage
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
