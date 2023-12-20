using System.Linq;

namespace DevTeam.Permissions.Core;

public static class MappingHelper
{
    public static string GetName(params int[] permissions)
    {
        return permissions.OrderBy(x => x).Aggregate(string.Empty, (result, current) => $@"{result}{current}");
    }
}
