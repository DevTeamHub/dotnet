using System.Collections.Generic;

namespace DevTeam.GenericRepository;

public interface IPermissionsService
{
    List<PermissionModel> GetCurrentAccountPermissions();
    string GetCurrentAccountId();
}