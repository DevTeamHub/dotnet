namespace DevTeam.GenericRepository.Tests.Context.SecurityContext.Entities;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsAdmin { get; set; }
}
