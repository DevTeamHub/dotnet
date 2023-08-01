using DevTeam.Session.Abstractions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace DevTeam.Session;

public class SessionService : ISessionService
{
    private readonly IHttpContextAccessor _contextAccessor;

    protected ISession Session => _contextAccessor.HttpContext.Session;

    public SessionService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public object? GetItem(Type type)
    {
        var data = Session.GetString(type.Name);

        return data != null ? JsonConvert.DeserializeObject(data, type) : null;
    }

    public TModel? GetItem<TModel>()
        where TModel : class
    {
        return (TModel?) GetItem(typeof(TModel));
    }

    public void AddItem(Type type, object model)
    {
        var sessionData = JsonConvert.SerializeObject(model);
        Session.SetString(type.Name, sessionData);
    }

    public void AddItem<TModel>(TModel model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        AddItem(typeof(TModel), model);
    }

    public void RemoveItem(Type type)
    {
        if (Session.Keys.Contains(type.Name))
            Session.Remove(type.Name);
    }

    public void RemoveItem<TModel>()
    {
        RemoveItem(typeof(TModel));
    }

    public void Clear()
    {
        Session.Clear();
    }
}