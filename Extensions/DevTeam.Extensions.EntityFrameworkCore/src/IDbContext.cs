using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;

namespace DevTeam.Extensions.EntityFrameworkCore;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : class;
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
        where TEntity : class;
    int SaveChanges();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
