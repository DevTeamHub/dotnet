using DevTeam.Extensions.Abstractions;
using DevTeam.GenericService;
using DevTeam.Session.Abstractions;
using DevTeam.UserContext.Asbtractions;
using System;
using System.Threading.Tasks;

namespace DevTeam.UserContext;

public interface IUserService<TKey>
    where TKey: struct
{
    Task SetUser(TKey userId);
}

public class UserService<TKey> : IUserService<TKey>
    where TKey : struct
{
    public Task SetUser(TKey userId)
    {
        return Task.CompletedTask;
    }
}

public class UserService<TUser, TUserModel, TKey> : IUserService<TKey>
    where TUser : class, IEntity<TKey>
    where TUserModel : class
    where TKey : struct, IEquatable<TKey>
{
    private readonly ISessionService? _sessionService;
    private readonly IGenericService _genericService;
    private readonly IUserContext<TUserModel, TKey> _userContext;

    public UserService(
        ISessionService? sessionService, 
        IGenericService genericService,
        IUserContext<TUserModel, TKey> userContext)
    {
        _sessionService = sessionService;
        _genericService = genericService;
        _userContext = userContext;
    }

    public async Task SetUser(TKey userId)
    {
        var user = _sessionService?.GetItem<TUserModel>();

        _userContext.User = user ?? await _genericService.GetAsync<TUser, TUserModel, TKey>(userId);
    }
}
