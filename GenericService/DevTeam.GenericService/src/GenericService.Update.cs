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
        _writeRepository.Save();
    }

    public virtual void UpdateProperty<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        _writeRepository.UpdateProperty(id, propertySelector, value);
        _writeRepository.Save();
    }

    public virtual void UpdateProperty<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity
    {
        _writeRepository.UpdateProperty(id, propertySelector, value);
        _writeRepository.Save();
    }

    public virtual void UpdateProperty<TEntity>(Expression<Func<TEntity, bool>> selector, string propertyName, object value)
        where TEntity : class
    {
        _writeRepository.UpdateProperty(selector, propertyName, value);
        _writeRepository.Save();
    }

    public virtual void UpdateProperty<TEntity, TKey>(TKey id, string propertyName, object value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        _writeRepository.UpdateProperty<TEntity, TKey>(id, propertyName, value);
        _writeRepository.Save();
    }

    public virtual void UpdateProperty<TEntity>(int id, string propertyName, object value)
        where TEntity : class, IEntity
    {
        _writeRepository.UpdateProperty<TEntity>(id, propertyName, value);
        _writeRepository.Save();
    }

    public virtual async Task UpdatePropertyAsync<TEntity, TProperty>(Expression<Func<TEntity, bool>> selector,
                                                                Expression<Func<TEntity, TProperty>> propertySelector,
                                                                TProperty value)
        where TEntity : class
    {
        await _writeRepository.UpdatePropertyAsync(selector, propertySelector, value);
        await _writeRepository.SaveAsync();
    }

    public virtual async Task UpdatePropertyAsync<TEntity, TProperty, TKey>(TKey id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        await _writeRepository.UpdatePropertyAsync(id, propertySelector, value);
        await _writeRepository.SaveAsync();
    }

    public virtual async Task UpdatePropertyAsync<TEntity, TProperty>(int id, Expression<Func<TEntity, TProperty>> propertySelector, TProperty value)
        where TEntity : class, IEntity
    {
        await _writeRepository.UpdatePropertyAsync(id, propertySelector, value);
        await _writeRepository.SaveAsync();
    }

    public virtual async Task UpdatePropertyAsync<TEntity>(Expression<Func<TEntity, bool>> selector, string propertyName, object value)
        where TEntity : class
    {
        await _writeRepository.UpdatePropertyAsync(selector, propertyName, value);
        await _writeRepository.SaveAsync();
    }

    public virtual async Task UpdatePropertyAsync<TEntity, TKey>(TKey id, string propertyName, object value)
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        await _writeRepository.UpdatePropertyAsync<TEntity, TKey>(id, propertyName, value);
        await _writeRepository.SaveAsync();
    }

    public virtual async Task UpdatePropertyAsync<TEntity>(int id, string propertyName, object value)
        where TEntity : class, IEntity
    {
        await _writeRepository.UpdatePropertyAsync<TEntity>(id, propertyName, value);
        await _writeRepository.SaveAsync();
    }

    #endregion
}
