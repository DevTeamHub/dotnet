namespace DevTeam.UserContext.Asbtractions;

public class UserContext<TKey> : IUserContext<TKey>
    where TKey: struct
{
    public TKey? Id { get; set; }
    public string? Role { get; set; }
    public bool IsAuthenticated => Id.HasValue;
}

public class UserContext<TUserModel, TKey> : UserContext<TKey>, IUserContext<TUserModel, TKey>
    where TUserModel : class
    where TKey : struct
{
    public TUserModel? User { get; set; }
}