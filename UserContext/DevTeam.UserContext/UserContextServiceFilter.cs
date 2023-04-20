using DevTeam.Extensions.Abstractions;
using DevTeam.GenericService;
using DevTeam.QueryMappings.Helpers;
using DevTeam.Session.Abstractions;
using DevTeam.UserContext.Asbtractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DevTeam.UserContext;

public abstract class UserContextBaseServiceFilter<TKey> : IAsyncActionFilter
    where TKey : struct
{
    protected abstract IUserContext<TKey> Context { get; }

    protected virtual Task SetUser(TKey userId, List<Claim> claims)
    {
        Context.Id = userId;

        var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
        if (role != null)
        {
            Context.Role = role.Value;
        }

        return Task.CompletedTask;
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
                await SetUser(userId, user.Claims.ToList());
            }
        }

        await next();
    }
}

public class UserContextServiceFilter<TKey> : UserContextBaseServiceFilter<TKey>
    where TKey : struct
{
    private readonly IUserContext<TKey> _userContext;

    public UserContextServiceFilter(IUserContext<TKey> userContext)
    {
        _userContext = userContext;
    }

    protected override IUserContext<TKey> Context => _userContext;
}

public class UserContextServiceFilter<TUser, TUserModel, TKey> : UserContextBaseServiceFilter<TKey>
    where TUser : class, IEntity<TKey>
    where TUserModel : class
    where TKey : struct, IEquatable<TKey>
{
    private readonly IUserContext<TUserModel, TKey> _userContext;
    private readonly ISessionService _sessionService;
    private readonly IMappingsList _mappings;
    private readonly IGenericService _service;

    public UserContextServiceFilter(
        IUserContext<TUserModel, TKey> userContext,
        ISessionService sessionService,
        IMappingsList mappings,
        IGenericService service)
    {
        _userContext = userContext;
        _sessionService = sessionService;
        _mappings = mappings;
        _service = service;
    }

    protected override IUserContext<TKey> Context => _userContext;

    protected override async Task SetUser(TKey userId, List<Claim> claims)
    {
        await base.SetUser(userId, claims);

        var user = _sessionService.GetItem<TUserModel>();

        _userContext.User = user ?? await GetUser(userId);
    }

    private async Task<TUserModel?> GetUser(TKey userId)
    {
        return _mappings.Exist<TUser, TUserModel>()
            ? await _service.GetAsync<TUser, TUserModel, TKey>(userId)
            : null;
    }
}
