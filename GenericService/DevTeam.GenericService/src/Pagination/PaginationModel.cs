using System.Collections.Generic;

namespace DevTeam.GenericService.Pagination;

public class PaginationModel<TModel>
{
    public List<TModel> List { get; }
    public int Count { get; }

    public PaginationModel(List<TModel> list, int count)
    {
        List = list;
        Count = count;
    }
}
