using System.Linq;

namespace DevTeam.GenericService.Pagination;

public interface ISearchService<TEntity, TSearchModel>
    where TEntity : class
    where TSearchModel : class
{
    IQueryable<TEntity> Filter(IQueryable<TEntity> query, TSearchModel searchModal);
}
