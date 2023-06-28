using System;

namespace DevTeam.Session.Abstractions;

public interface ISessionService
{
    object? GetItem(Type type);
    TModel? GetItem<TModel>()
        where TModel : class;
    void AddItem(Type type, object model);
    void AddItem<TModel>(TModel model);
    void RemoveItem(Type type);
    void RemoveItem<TModel>();
    void Clear();
}
