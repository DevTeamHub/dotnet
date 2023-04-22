using DevTeam.UserContext.Tests.Context.Entities;

namespace DevTeam.UserContext.Tests.Tests;

public static class TestData
{
    private static readonly List<User> _users = new()
        {
            new User
            {
                Id = 1,
                UserName = "User1",
                Password = "asdasdasdas",
                IsAdmin = false
            },
            new User
            {
                Id = 2,
                UserName = "User2",
                Password = "erterterte",
                IsAdmin = true
            },
            new User
            {
                Id = 3,
                UserName = "User3",
                Password = "ghjghjgjhjgh",
                IsAdmin = false
            }
        };

    public static List<User> Users => _users;
}
