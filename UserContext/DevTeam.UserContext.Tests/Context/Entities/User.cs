using DevTeam.Extensions.Abstractions;

namespace DevTeam.UserContext.Tests.Context.Entities;

public class User: IEntity
{
    public int Id { get; set; }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public bool IsAdmin { get; set; }
}