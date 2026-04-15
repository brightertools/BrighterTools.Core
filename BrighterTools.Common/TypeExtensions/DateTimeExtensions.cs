namespace App.TypeExtensions;

public static class DateTimeExtensions
{
    public static int ToUnixTimestamp(this DateTime value)
    {
        return (int)(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }

    public static DateTime? GetLatestDate(this DateTime? current, params DateTime?[] dates)
    {
        // Combine the current date with the list of dates, handling null for current date
        var allDates = new DateTime?[] { current }.Concat(dates);

        // Filter out null values and return the maximum date, or null if all are null
        var nonNullDates = allDates.Where(date => date.HasValue);
        return nonNullDates.Any() ? nonNullDates.Max() : (DateTime?)null;
    }
}
