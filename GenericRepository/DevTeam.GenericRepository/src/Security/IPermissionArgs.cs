using System;
using System.Collections.Generic;
using System.Text;

namespace DevTeam.GenericRepository;

public interface IPermissionsArgs
{
    int AccessPermission { get; set; }
    int[] OtherPermissions { get; set; }
}
