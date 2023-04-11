namespace DevTeam.QueryMappings.Tests.Context.RentalContext.Models;

public class UserModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsAdmin { get; set; }
}
