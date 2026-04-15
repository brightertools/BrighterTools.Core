using Microsoft.Extensions.Configuration;

namespace App.TypeExtensions;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Extension method to get a comma or semicolon separated string and return it as a List of strings.
    /// </summary>
    /// <param name="configuration">The IConfiguration instance</param>
    /// <param name="key">The key in appsettings.json</param>
    /// <returns>A List of strings representing the separated values</returns>
    public static List<string> GetList(this IConfiguration configuration, string key)
    {
        var value = configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            return [];
        }

        return value.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
    }

    /// <summary>
    /// Extension method to get a value from appsettings and convert it to the specified enum type.
    /// </summary>
    /// <typeparam name="TEnum">The type of the enum</typeparam>
    /// <param name="configuration">The IConfiguration instance</param>
    /// <param name="key">The key in appsettings.json</param>
    /// <returns>The corresponding enum value</returns>
    public static TEnum GetEnumValue<TEnum>(this IConfiguration configuration, string key) where TEnum : struct, Enum
    {
        var value = configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"The key '{key}' was not found in the configuration or is empty.");
        }

        // Try to parse the string value into the enum
        if (Enum.TryParse<TEnum>(value, true, out var result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException($"The value '{value}' for key '{key}' is not a valid {typeof(TEnum).Name}.");
        }
    }
}