using System.ComponentModel.DataAnnotations;

namespace DevTeam.GenericRepository.Tests.Context.RentalContext.Entities;

public class Review
{
    [Key]
    public int Id { get; set; }
    public int EntityId { get; set; }
    public int EntityTypeId { get; set; }
    public int Rating { get; set; }
    public string? Comments { get; set; } 
}
