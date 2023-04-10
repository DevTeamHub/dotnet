using DevTeam.Extensions.Abstractions;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DevTeam.GenericService;

public partial class GenericService<TContext>
{
    #region Delete

    public virtual int Delete<TEntity>(int id)
        where TEntity : class, IEntity
    {
        _writeRepository.Delete<TEntity>(id);
        return _writeRepository.Save();
    }

    public virtual int Delete<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        _writeRepository.Delete(filter);
        return _writeRepository.Save();
    }

    public virtual async Task<int> DeleteAsync<TEntity>(int id)
        where TEntity : class, IEntity
    {
        await _writeRepository.DeleteAsync<TEntity>(id);
        return await _writeRepository.SaveAsync();
    }

    public virtual async Task<int> DeleteAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        await _writeRepository.DeleteAsync(filter);
        return await _writeRepository.SaveAsync();
    }

    #endregion

    #region Delete Range

    public virtual int DeleteRange<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        _writeRepository.DeleteRange(filter);
        return _writeRepository.Save();
    }

    public virtual async Task<int> DeleteRangeAsync<TEntity>(Expression<Func<TEntity, bool>> filter)
        where TEntity : class
    {
        await _writeRepository.DeleteRangeAsync(filter);
        return await _writeRepository.SaveAsync();
    }

    #endregion
}
