namespace DevTeam.Permissions.Core;

public class PermissionModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public int? EntityType { get; set; } 
    public int? Scopes { get; set; }
    public int? ContainerType { get; set; }
}
