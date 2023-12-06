using System.Collections.Generic;
using System.Threading.Tasks;
using DevTeam.Extensions.Abstractions;

namespace DevTeam.GenericRepository;

public interface IRelatedDataService<TRelatedData>
where TRelatedData : RelatedData
{
    TRelatedData GetCurrentAccountRelatedData();
}