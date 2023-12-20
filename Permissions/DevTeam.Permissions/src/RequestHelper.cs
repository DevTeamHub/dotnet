using DevTeam.Permissions.Core.Attributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DevTeam.Permissions;

public static class RequestHelper
{
    public static List<RequestId> GetIdsFromRequest(this ActionExecutingContext context)
    {
        var requestIds = context.ActionArguments.Values
            .Where(v => v.GetType().IsClass)
            .SelectMany(x => x.Items<object>())
            .SelectMany(GetPropertyValues)
            .Where(x => x.Ids != null)
            .SelectMany(x => x.Ids!.Items<int>().Select(y => new RequestId { Id = y, Type = x.Type }));

        if (
            context.ActionDescriptor.Parameters.Select(x => (ControllerParameterDescriptor)x)
                is IEnumerable<ControllerParameterDescriptor> parameters
        )
        {
            var ids = parameters
                .Select(x => new { Parameter = x, Attribute = x.ParameterInfo.GetCustomAttribute<IdAttribute>() })
                .Where(x => x.Attribute != null)
                .Select(x => new RequestId { Id = Convert.ToInt32(context.RouteData.Values[x.Parameter.Name]), Type = x.Attribute!.EntityType });

            requestIds = requestIds.Concat(ids);
        }

        return requestIds
            .GroupBy(x => new { x.Id, x.Type })
            .Select(x => x.Key)
            .Select(x => new RequestId { Id = x.Id, Type = x.Type })
            .ToList();
    }

    private static IEnumerable<RequestIdProperty> GetPropertyValues(object model)
    {
        return model.GetType()
            .GetProperties()
            .Where(p => p.GetCustomAttribute<IdAttribute>() != null)
            .Select(p => new RequestIdProperty { Ids = p.GetValue(model), Type = p.GetCustomAttribute<IdAttribute>()!.EntityType });
    }

    private static IEnumerable<TItem> Items<TItem>(this object value)
    {
        if (value is IEnumerable<TItem> items)
        {
            return items;
        }
        else if (value is TItem item)
        {
            return new List<TItem> { item };
        }

        return Enumerable.Empty<TItem>();
    }
}
