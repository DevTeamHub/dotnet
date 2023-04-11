namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Models;

public class PersonModel
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
