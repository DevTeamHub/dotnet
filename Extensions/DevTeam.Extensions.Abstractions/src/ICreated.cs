using System;

namespace DevTeam.Extensions.Abstractions;

public interface ICreated<TKey>
{
    TKey CreatedBy { get; set; }
    DateTime CreatedOn { get; set; }
}

public interface ICreated : ICreated<int>
{ }
