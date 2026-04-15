using Microsoft.Extensions.Logging;

#pragma warning disable CA2254 // Template should be a static expression

namespace App.TypeExtensions;

public static class LoggerExtensions
{
    /// <summary>
    /// Logs a Debug message unless the error Count is greater than the suppression count, in which case it logs an Error message.
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="errorMessagePrefix"></param>
    /// <param name="errorCount"></param>
    /// <param name="suppressionCount"></param>
    /// <returns>Returns true if Error Log is created, ie error count is greater than suppresssion count</returns>
    public static bool LogErrorWithSuppression(this ILogger logger, Exception exception, string errorMessagePrefix = "", int errorCount = 0, int suppressionCount = 0)
    {
        var logError = errorCount > suppressionCount;

        if (logError)
        {
            logger.LogError($"{errorMessagePrefix} count={errorCount}, exception:{exception.Message}, stack-trace{exception.StackTrace}");

            if (exception.InnerException != null)
            {
                logger.LogError($"{errorMessagePrefix} count={errorCount}, inner-exception:{exception.Message}, stack-trace{exception.StackTrace}");
            }

            return true;
        }

        logger.LogDebug($"{errorMessagePrefix} count={errorCount}, exception:{exception.Message}, stack-trace{exception.StackTrace}");

        if (exception.InnerException != null)
        {
            logger.LogDebug($"{errorMessagePrefix} count={errorCount}, inner-exception:{exception.Message}, stack-trace{exception.StackTrace}");
        }
        return false;
    }

    /// <summary>
    /// Logs App Error, logging exception (and inner exception if available) and stack trace
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="exception"></param>
    /// <param name="errorMessagePrefix"></param>
    public static void LogAppError(this ILogger logger, Exception exception, string errorMessagePrefix = "")
    {
        var message = $"{errorMessagePrefix}, exception:{exception.Message}, stack-trace{exception.StackTrace}";
        logger.LogError(message);


        if (exception.InnerException != null)
        {
            message = $"{errorMessagePrefix}, inner-exception:{exception.InnerException.Message}, stack-trace{exception.InnerException.StackTrace}";
            logger.LogError(message);
        }
    }
}
