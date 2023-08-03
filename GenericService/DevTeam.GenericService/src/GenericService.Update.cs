using DevTeam.Extensions.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevTeam.GenericService;

public partial class GenericService<TContext>
{
    #region Update

    public virtual TEntity? Update<TModel, TEntity, TKey>(TKey id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        var entity = _writeRepository.Get<TEntity, TKey>(id);

        if (entity == null) return null;

        updateFunc(model, entity);
        _writeRepository.Save();

        return entity;
    }

    public virtual TResult? Update<TModel, TEntity, TResult, TKey>(TKey id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        Update(id, model, updateFunc);
        return Get<TEntity, TResult, TKey>(id);
    }

    public virtual TResult? Update<TModel, TEntity, TResult>(int id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity
        where TResult : class
    {
        Update(id, model, updateFunc);
        return Get<TEntity, TResult>(id);
    }

    public virtual async Task<TEntity?> UpdateAsync<TModel, TEntity, TKey>(TKey id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        var entity = await _writeRepository.GetAsync<TEntity, TKey>(id);

        if (entity == null) return null;

        updateFunc(model, entity);
        await _writeRepository.SaveAsync();

        return entity;
    }

    public virtual async Task<TResult?> UpdateAsync<TModel, TEntity, TResult, TKey>(TKey id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        await UpdateAsync(id, model, updateFunc);
        return await GetAsync<TEntity, TResult, TKey>(id);
    }

    public virtual async Task<TResult?> UpdateAsync<TModel, TEntity, TResult>(int id, TModel model, Action<TModel, TEntity> updateFunc)
        where TEntity : class, IEntity
        where TResult : class
    {
        await UpdateAsync(id, model, updateFunc);
        return await GetAsync<TEntity, TResult>(id);
    }

    public virtual void UpdateProperty<TEntity, TProperty>(Expression<Func<TEntity, bool>> selector,
                                                           Expression<Func<TEntity, TProperty>> propertySelector,
                                                           TProperty value)
        where TEntity : class
    {
        _writeRepository.UpdateProperty(selector, propertySelector, value);
    }

    public virtual void UpdateProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        _writeRepository.UpdateProperty(id, propertySelector, value);
    }

    public virtual void UpdateProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity
    {
        _writeRepository.UpdateProperty(id, propertySelector, value);
    }

    public virtual void UpdateProperty<TEntity>(Expression<Func<TEntity, bool>> selector, string propertyName, object value)
        where TEntity : class
    {
        _writeRepository.UpdateProperty(selector, propertyName, value);
    }

    public virtual void UpdateProperty<TEntity, TKey>(TKey id, string propertyName, object value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        _writeRepository.UpdateProperty<TEntity, TKey>(id, propertyName, value);
    }

    public virtual void UpdateProperty<TEntity>(int id, string propertyName, object value)
        where TEntity : class, IEntity
    {
        UpdateProperty<TEntity, int>(id, propertyName, value);
    }

    public virtual Task UpdatePropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> selector,
                                                                Expression<Func<TEntity, TProperty>> propertySelector,
                                                                TProperty value)
        where TEntity : class
    {
        return _writeRepository.UpdatePropertyAsync(selector, propertySelector, value);
    }

    public virtual Task UpdatePropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return _writeRepository.UpdatePropertyAsync(id, propertySelector, value);
    }

    public virtual Task UpdatePropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity
    {
        return _writeRepository.UpdatePropertyAsync(id, propertySelector, value);
    }

    public virtual Task UpdatePropertyAsync<TEntity>(Expression<Func<TEntity, bool>> selector, string propertyName, object value)
        where TEntity : class
    {
        return _writeRepository.UpdatePropertyAsync(selector, propertyName, value);
    }

    public virtual Task UpdatePropertyAsync<TEntity, TKey>(TKey id, string propertyName, object value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        return _writeRepository.UpdatePropertyAsync<TEntity, TKey>(id, propertyName, value);
    }

    public virtual Task UpdatePropertyAsync<TEntity>(int id, string propertyName, object value)
        where TEntity : class, IEntity
    {
        return _writeRepository.UpdatePropertyAsync<TEntity, int>(id, propertyName, value);
    }

    #endregion
}
