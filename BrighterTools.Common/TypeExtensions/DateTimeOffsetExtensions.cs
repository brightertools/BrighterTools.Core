namespace App.TypeExtensions;

public static class DateTimeOffsetExtensions
{
    /// <summary>
    /// Gets the Age at the time of reference
    /// Use DateTimeOffset.Now to get age now or future date to get age then
    /// </summary>
    /// <param name="birthday"></param>
    /// <param name="reference"></param>
    /// <returns></returns>
    public static int GetAge(this DateTimeOffset birthday, DateTimeOffset reference)
    {
        var age = reference.Year - birthday.Year;
        if (reference < birthday.AddYears(age)) age--;
        return age;
    }

    /// <summary>
    ///     Sets the time of the current date with minute precision.
    /// </summary>
    /// <param name="current">The current date.</param>
    /// <param name="hour">The hour.</param>
    /// <returns>A DateTimeOffset.</returns>
    public static DateTimeOffset SetTime(this DateTimeOffset current, int hour)
    {
        return SetTime(current, hour, 0, 0, 0);
    }

    /// <summary>
    ///     Sets the time of the current date with minute precision.
    /// </summary>
    /// <param name="current">The current date.</param>
    /// <param name="hour">The hour.</param>
    /// <param name="minute">The minute.</param>
    /// <returns>A DateTimeOffset.</returns>
    public static DateTimeOffset SetTime(this DateTimeOffset current, int hour, int minute)
    {
        return SetTime(current, hour, minute, 0, 0);
    }

    /// <summary>
    ///     Sets the time of the current date with second precision.
    /// </summary>
    /// <param name="current">The current date.</param>
    /// <param name="hour">The hour.</param>
    /// <param name="minute">The minute.</param>
    /// <param name="second">The second.</param>
    /// <returns>A DateTimeOffset.</returns>
    public static DateTimeOffset SetTime(this DateTimeOffset current, int hour, int minute, int second)
    {
        return SetTime(current, hour, minute, second, 0);
    }

    /// <summary>
    ///     Sets the time of the current date with millisecond precision.
    /// </summary>
    /// <param name="current">The current date.</param>
    /// <param name="hour">The hour.</param>
    /// <param name="minute">The minute.</param>
    /// <param name="second">The second.</param>
    /// <param name="millisecond">The millisecond.</param>
    /// <returns>A DateTimeOffset.</returns>
    public static DateTimeOffset SetTime(this DateTimeOffset current, int hour, int minute, int second, int millisecond)
    {
        return new DateTime(current.Year, current.Month, current.Day, hour, minute, second, millisecond);
    }

    public static DateTimeOffset? GetLatestDate(this DateTimeOffset? current, params DateTimeOffset?[] dates)
    {
        // Combine the current date with the list of dates, handling null for current date
        var allDates = new DateTimeOffset?[] { current }.Concat(dates);

        // Filter out null values and return the maximum date, or null if all are null
        var nonNullDates = allDates.Where(date => date.HasValue);
        return nonNullDates.Any() ? nonNullDates.Max() : (DateTimeOffset?)null;
    }
}
