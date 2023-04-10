using DevTeam.Extensions.EntityFrameworkCore;
using DevTeam.QueryMappings.Tests.Context.SecurityContext.Entities;
using DevTeam.QueryMappings.Tests.Tests;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DevTeam.QueryMappings.Tests.Mappings
{
    public interface ISecurityContext: IDbContext { }

    public class SecurityContext : DbContext, ISecurityContext
    {
        public IEnumerable<User> Users => TestData.Users;
    }
}
