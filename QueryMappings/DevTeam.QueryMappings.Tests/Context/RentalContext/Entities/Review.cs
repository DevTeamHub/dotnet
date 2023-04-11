namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Entities;

public class Review
{
    public int Id { get; set; }
    public int EntityId { get; set; }
    public int EntityTypeId { get; set; }
    public int Rating { get; set; }
    public string? Comments { get; set; }
}
