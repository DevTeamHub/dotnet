using System.Collections.Generic;

namespace DevTeam.Permissions.Core;

public interface IPermissionsService
{
    List<PermissionModel> GetCurrentAccountPermissions();
    string GetCurrentAccountId();
}
