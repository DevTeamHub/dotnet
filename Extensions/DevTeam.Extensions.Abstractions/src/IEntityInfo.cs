namespace DevTeam.Extensions.Abstractions;

public interface IEntityInfo<TKey, TType>
{
    TKey EntityId { get; set; }
    TType EntityTypeId { get; set; }
}

public interface IEntityInfo : IEntityInfo<int, int>
{ }
