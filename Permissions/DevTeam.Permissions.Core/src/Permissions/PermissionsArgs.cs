using DevTeam.GenericService;
namespace DevTeam.Permissions.Core;

public class PermissionsArgs : IPermissionsArgs, IServiceArgs
{
    public int AccessPermission { get; set; }
    public int[] OtherPermissions { get; set; } = null!;
    public ArgumentType Type { get; set; } = ArgumentType.Query;
}
