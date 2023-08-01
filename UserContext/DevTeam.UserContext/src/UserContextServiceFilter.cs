using DevTeam.UserContext.Asbtractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevTeam.UserContext;

public class UserContextServiceFilter<TKey> : IAsyncActionFilter
    where TKey : struct
{
    private readonly IUserContext<TKey> _userContext;
    private readonly IUserService<TKey> _userService;   

    public UserContextServiceFilter(
        IUserContext<TKey> userContext,
        IUserService<TKey> userService) 
    {
        _userContext = userContext;
        _userService = userService;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var user = context.HttpContext.User;

        if (user.Identity != null && user.Identity.IsAuthenticated)
        {
            var subject = user.Identity.Name;

            if (!string.IsNullOrEmpty(subject))
            {
                var userId = (TKey) Convert.ChangeType(subject, typeof(TKey));

                _userContext.Id = userId;

                var role = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

                if (role != null)
                {
                    _userContext.Role = role.Value;
                }

                await _userService.SetUser(userId);
            }
        }

        await next();
    }
}
