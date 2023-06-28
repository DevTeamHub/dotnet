using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DevTeam.Extensions.Helpers;

public static class ReflectionHelper
{
    public static void Update<TEntity>(TEntity target, TEntity source)
    {
        typeof(TEntity).GetProperties().ToList().ForEach(property =>
        {
            var value = property.GetValue(source);
            property.SetValue(target, value);
        });
    }

    public static void UpdateMany<TEntity>(List<TEntity> targetItems, List<TEntity> sourceItems, Expression<Func<TEntity, int>> compareBy)
    {
        sourceItems.ForEach(source =>
        {
            var target = targetItems.Single(t => ExpressionHelper.Compare(t, source, compareBy));
            Update(target, source);
        });
    }
}
