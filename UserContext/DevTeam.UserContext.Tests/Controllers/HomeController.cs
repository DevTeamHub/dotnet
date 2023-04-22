using DevTeam.UserContext.Asbtractions;
using DevTeam.UserContext.Tests.Context.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevTeam.UserContext.Tests.Controllers;

public class HomeController: ControllerBase
{
    private readonly IUserContext<UserModel, int> _userContext;

    public IUserContext<UserModel, int> UserContext => _userContext;

    public HomeController(IUserContext<UserModel, int> userContext) 
    {
        _userContext = userContext;
    }
}
