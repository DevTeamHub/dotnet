using System;
using System.Linq.Expressions;

namespace DevTeam.Helpers;

public static class ExpressionHelper
{
    public static Expression<Func<TEntity, bool>> Compare<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertySelector, TProperty property)
    {
        throw new NotImplementedException();
    }

    public static bool Compare<TItem>(TItem item1, TItem item2, Expression<Func<TItem, int>> property)
    {
        var selector = property.Compile();
        return selector(item1) == selector(item2);
    }
}
