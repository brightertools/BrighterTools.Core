using System.Web;
using System.Text;

namespace App.TypeExtensions;

public static class DictionaryExtensions
{
    public static string ToCallbackPostDataString(this Dictionary<string, string> value)
    {
        var result = new StringBuilder();

        foreach (var pair in value)
        {
            result.AppendFormat("{0}{1}={2}", result.Length > 0 ? "&" : string.Empty, char.ToLowerInvariant(pair.Key[0]) + pair.Key.Substring(1), HttpUtility.UrlEncode(pair.Value));
        }

        return result.ToString();
    }
}