namespace App.TypeExtensions;

public static class ListExtensions
{
    public static void Sort<T, U>(this List<T> list, Func<T, U> expression, bool assesnding = true) where U : IComparable<U>
    {
        if (assesnding)
        {
            list.Sort((x, y) => expression.Invoke(x).CompareTo(expression.Invoke(y)));
        }
        else
        {
            list.Sort((x, y) => expression.Invoke(y).CompareTo(expression.Invoke(x)));
        }
    }

    public static string ToDelimitedString<T>(this List<T> list, string separator, bool removeEmptyValues = true)
    {
        var result = string.Empty;

        if (list == null || list.Count <= 0)
        {
            return result;
        }

        foreach (var item in list)
        {
            var value = (item?.ToString() ?? "").Trim();

            if (removeEmptyValues && string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            result = $"{result}{separator}{value}";
        }

        return result.TrimEnd(separator);
    }
}
