using System.Linq;

namespace DevTeam.GenericRepository;

public interface IQueryExtension
{
    public IQueryable<TEntity> Apply<TEntity>(IQueryable<TEntity> query); 
}
