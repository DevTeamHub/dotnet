using System;

namespace DevTeam.Permissions.Core.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CanPerformAttribute : Attribute
{
    public int Permission { get; }

    public CanPerformAttribute(int permission)
    {
        Permission = permission;
    }
}
