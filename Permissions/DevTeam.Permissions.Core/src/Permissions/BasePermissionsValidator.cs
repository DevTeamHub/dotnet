using System.Collections.Generic;

namespace DevTeam.Permissions.Core;

public abstract class BasePermissionsValidator<TRelatedData>
{
    public abstract int Scope { get; }
    public abstract bool IsAllowed(List<TRelatedData> data);
}
