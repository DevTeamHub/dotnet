using System;

namespace DevTeam.Permissions.Core.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter)]
public class IdAttribute : Attribute
{
    public Type EntityType { get; }

    public IdAttribute(Type type)
    {
        EntityType = type;
    }
}
