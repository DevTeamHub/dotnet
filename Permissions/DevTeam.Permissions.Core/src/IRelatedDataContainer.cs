namespace DevTeam.Permissions.Core;

public interface IRelatedDataContainer<TRelatedData>
{
    public TRelatedData RelatedData { get; set; }
}
