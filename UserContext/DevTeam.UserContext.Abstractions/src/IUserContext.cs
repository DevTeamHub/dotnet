namespace DevTeam.UserContext.Asbtractions;

public interface IUserContext<TKey>
    where TKey : struct
{
    TKey? Id { get; set; }
    string? Role { get; set; }
    bool IsAuthenticated { get; }
}

public interface IUserContext<TUserModel, TKey> : IUserContext<TKey>
    where TUserModel : class
    where TKey : struct
{
    TUserModel? User { get; set; }
}