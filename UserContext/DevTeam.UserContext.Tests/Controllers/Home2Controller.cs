using DevTeam.UserContext.Asbtractions;
using DevTeam.UserContext.Tests.Context.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevTeam.UserContext.Tests.Controllers;

public class Home2Controller: ControllerBase
{
    private readonly IUserContext<int> _userContext;

    public IUserContext<int> UserContext => _userContext;

    public Home2Controller(IUserContext<int> userContext) 
    {
        _userContext = userContext;
    }
}
