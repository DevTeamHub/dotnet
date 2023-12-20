using DevTeam.Extensions.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevTeam.Permissions.Core;

public interface IRelatedDataService<TRelatedData>
    where TRelatedData : RelatedData
{
    Task<List<TRelatedData>> GetRelatedData<TEntity>(List<int> requestIds)
        where TEntity : class, IEntity;

    TRelatedData GetCurrentAccountRelatedData();
}
