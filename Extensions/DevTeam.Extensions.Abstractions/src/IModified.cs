using System;

namespace DevTeam.Extensions.Abstractions;

public interface IModified<TKey>
{
    TKey ModifiedBy { get; set; }
    DateTime ModifiedOn { get; set; }
}

public interface IModified : IModified<int>
{ }
