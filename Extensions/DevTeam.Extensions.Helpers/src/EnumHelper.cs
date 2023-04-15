using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DevTeam.Extensions.Helpers;

public static class EnumHelper
{
    public static string Description<TEnum>(this TEnum value)
        where TEnum: struct, Enum
    {
        if (!value.GetType().IsEnum)
            throw new ArgumentException("Type must be Enum");

        var description = GetAttribute<DescriptionAttribute>(value);
        return description != null ? description.Description : string.Empty;
    }

    public static TAttribute? GetAttribute<TAttribute>(this Enum value)
        where TAttribute : Attribute
    {
        var type = value.GetType();
        var name = Enum.GetName(type, value);

        if (string.IsNullOrEmpty(name)) return null;

        return type.GetField(name)!
            .GetCustomAttributes(false)
            .OfType<TAttribute>()
            .SingleOrDefault();
    }

    public static List<TEnum> GetFlags<TEnum>(TEnum input)
        where TEnum: Enum
    {
        return Enum.GetValues(input.GetType())
            .Cast<TEnum>()
            .Where(value => input.HasFlag(value))
            .ToList();
    }
}