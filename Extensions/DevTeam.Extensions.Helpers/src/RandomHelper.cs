using System;
using System.Linq;

namespace DevTeam.Extensions.Helpers;

public static class RandomHelper
{
    private static readonly Random random = new Random();

    public static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray());
    }
}