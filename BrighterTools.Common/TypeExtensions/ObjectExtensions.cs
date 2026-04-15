using System.ComponentModel;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace App.TypeExtensions;

public static class ObjectExtensions
{
    public static string ToJson(this object value, bool indented = false)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, // This will prevent certain characters (including +, &, <, >, etc.) from being escaped during serialization.
                WriteIndented = indented,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            return JsonSerializer.Serialize(value, options);
        }
        catch
        {
            return "";
        }
    }

    public static Dictionary<string, string> ToDictionary<T>(this T value)
    {
        if (value == null)
        {
            return [];
        }

        return value.GetType().GetProperties().ToDictionary(x => x.Name, x => x.GetValue(value)?.ToString() ?? "");
    }

    public static byte[] ToJsonAsByteArray(this object value)
    {
        return Encoding.ASCII.GetBytes(value.ToJson());
    }

    public static dynamic ToDynamic(this object value)
    {
        var expando = new ExpandoObject() as IDictionary<string, object?>;

        foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
        {
            expando[property.Name] = property.GetValue(value);
        }

        return expando as dynamic;
    }

    public static void SetPropertyValue(this object obj, string propName, object value)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj), "Object cannot be null.");
        }

        if (string.IsNullOrEmpty(propName))
        {
            throw new ArgumentException("Property name cannot be null or empty.", nameof(propName));
        }

        var propertyInfo = obj.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Property '{propName}' not found on type '{obj.GetType().Name}'.");
        }

        // Ensure the value is of the correct type
        if (value != null && !propertyInfo.PropertyType.IsAssignableFrom(value.GetType()))
        {
            throw new ArgumentException($"Value type '{value.GetType().Name}' is not assignable to property type '{propertyInfo.PropertyType.Name}'.");
        }

        propertyInfo.SetValue(obj, value);
    }
}
