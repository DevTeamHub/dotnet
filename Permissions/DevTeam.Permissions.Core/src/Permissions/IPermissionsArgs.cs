namespace DevTeam.Permissions.Core;

public interface IPermissionsArgs
{
    int AccessPermission { get; set; }
    int[] OtherPermissions { get; set; }
}
