using System.Globalization;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace App.TypeExtensions;

public static class StringExtensions
{
    private static string _htmlTagsRegex = "<.*?>";

    public static string UppercaseFirstLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        return char.ToUpper(input[0]) + input.Substring(1);
    }


    public static string LowercaseFirstLetter(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }   

        return char.ToLower(input[0]) + input.Substring(1);
    }

    public static string Truncate(this string value, int maxLength)
    {
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    /// <summary>
    /// Separates a string in pascal case into separate words.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string SplitPascalCaseWords(this string value)
    {
        return SplitPascalCaseWords(value, false);
    }
    /// <summary>
    /// Separates a string in pascal case into separate words.
    /// Set useNonBreakingSpace to return string with html non breaking spaces
    /// </summary>
    /// <param name="value"></param>
    /// <param name="useNonBreakingSpace"></param>
    /// <returns></returns>
    public static string SplitPascalCaseWords(this string value, bool useNonBreakingSpace)
    {
        return System.Text.RegularExpressions.Regex.Replace(value, "([A-Z][A-Z]*)", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim().Replace(" ", useNonBreakingSpace ? "&nbsp;" : " ");
    }

    /// <summary>
    /// Convert delimited string to int list
    /// </summary>
    /// <param name="values"></param>
    /// <param name="delimiter"></param>
    /// <returns></returns>
    public static IEnumerable<int> ToIntList(this string values, char delimiter = ',')
    {
        if (String.IsNullOrEmpty(values)) yield break;

        foreach (var value in values.Split(delimiter))
        {
            int num;
            if (int.TryParse(value, out num))
            {
                yield return num;
            }
        }
    }

    public static string TrimEnd(this string value, string endString)
    {
        return !string.IsNullOrWhiteSpace(value.Trim()) && !string.IsNullOrWhiteSpace(endString.Trim()) && !value.Trim().EndsWith(endString) ? value : value.Remove(value.Trim().LastIndexOf(endString.Trim(), StringComparison.Ordinal));
    }

    public static string FirstLetterToUpper(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }

        if (value.Length > 1)
        {
            return char.ToUpper(value[0]) + value.Substring(1);
        }

        return value.ToUpper();
    }

    public static string Replace(this string s, char[] separators, string newVal)
    {
        var temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(newVal, temp);
    }

    public static string GetFileNameFromUrl(this string value)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out Uri? uri))
        {
            Uri dummyBaseUri = new("https://dummy");
            uri = new Uri(dummyBaseUri, value);
        }

        return Path.GetFileName(uri.LocalPath);
    }

    public static string SanitizeFilename(this string name)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
        var result = System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
        return result.Replace(" ", "_").Replace(".", "_").Replace("__", "_").Replace("__", "_");
    }

    public static string EnsureHttpsWebAddress(this string? value)
    {
        var webAddress = (value ?? "").Trim();

        if (webAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return webAddress;
        }

        if (webAddress.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
        {
            return "https://" + webAddress.Substring("http://".Length);
        }

        return string.IsNullOrWhiteSpace(webAddress) ? "" : "https://" + webAddress;
    }

    public static string RemoveAccents(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return text;
        }

        text = text.Normalize(NormalizationForm.FormD);
        char[] chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();

        return new string(chars).Normalize(NormalizationForm.FormC);
    }

    public static string ToUrlSlug(this string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        // Convert to lower case
        value = value.ToLowerInvariant();

        // Remove all accents and make the string lower case.  
        value = value.RemoveAccents().ToLower();

        // Replace spaces with dashes
        value = Regex.Replace(value, @"\s+", "-");

        // Remove invalid characters
        value = Regex.Replace(value, @"[^a-z0-9\-]", "");

        // Trim dashes and double dashes from the start and end
        value = value.Trim('-').Replace("--", "-");

        return value;
    }

    public static string SanitizeTextInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input) is true) return input;

        var sanitizedOutput = Regex.Replace(input, _htmlTagsRegex, string.Empty);

        return sanitizedOutput;
    }

    public static string FormatWebAddress(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        value = value.Trim().ToLower();

        return value.StartsWith("http") ? value : $"http://{value}";
    }

    public static bool IsNotEmptyId(this string value)
    {
        return !IsEmptyId(value);
    }

    public static bool IsEmptyId(this string value)
    {
        return string.IsNullOrWhiteSpace(value) || value.Equals(Guid.Empty.ToString());
    }

    public static bool IsGuid(this string value)
    {
        return Guid.TryParse(value, out var _);
    }

    public static bool EndsWithOneOf(this string value, IEnumerable<string> suffixes)
    {
        return suffixes.Any(value.EndsWith);
    }

    public static string HtmlEncode(this string value)
    {
        return WebUtility.HtmlEncode(value).Replace("{{", "").Replace("}}", "");
    }
}