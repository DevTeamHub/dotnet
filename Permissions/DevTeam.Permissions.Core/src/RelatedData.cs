using System.Collections.Generic;

namespace DevTeam.Permissions.Core;

public abstract class RelatedData
{
    public int EntityId { get; set; }
    public int EntityType { get; set; } = default!;

    public abstract List<int> GetIdsByType(int entityType);
}
