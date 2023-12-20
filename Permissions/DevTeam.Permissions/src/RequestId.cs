using System;

namespace DevTeam.Permissions;

public class RequestIdProperty
{
    public object? Ids { get; set; } = null!;
    public Type Type { get; set; } = null!;
}

public class RequestId
{
    public int Id { get; set; } = default!;
    public Type Type { get; set; } = null!;
}
