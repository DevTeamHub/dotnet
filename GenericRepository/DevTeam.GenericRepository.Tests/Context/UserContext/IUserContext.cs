using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTeam.GenericRepository.Tests.Context
{
    public interface IUserContext
    {
        int? Id { get; set; }
        string? Role { get; set; }
        bool IsAuthenticated { get; }
    }

    public interface IUserContext<TUserModel> : IUserContext
        where TUserModel : class
    {
        TUserModel? User { get; set; }
    }
}
