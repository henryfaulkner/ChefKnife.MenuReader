using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChefKnife.MenuReader.Core;

public static class StringExtensions
{
    // https://andrewlock.net/customising-asp-net-core-identity-ef-core-naming-conventions-for-postgresql/
    public static string? ToSnakeCase(this string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        var startUnderscores = Regex.Match(input, @"^_+");
        return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }
}
