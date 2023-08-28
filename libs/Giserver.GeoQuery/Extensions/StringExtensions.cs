namespace Giserver.GeoQuery.Extensions;

public static class StringExtensions
{
    public static void ThrowIfNullOrWhiteSpace(this string? value, string name)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(name);
    }

    public static string[]? SpliteByComma(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return value.Split(',');
    }

    public static string Format(this string value, params object?[] args)
    {
        return string.Format(value, args);
    }
}