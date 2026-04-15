using Microsoft.AspNetCore.Http;

namespace App.TypeExtensions;

public static class HttpRequestExtensions
{
    public static string? GetClientIpAddress(this HttpRequest request)
    {
        if (request.Headers.TryGetValue("X-Forwarded-For", out var forwardedFor))
        {
            // The X-Forwarded-For header can contain multiple IP addresses separated by commas.
            // The first IP address is the original client IP.
            var ip = forwardedFor.FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim();
            if (!string.IsNullOrEmpty(ip))
            {
                return ip;
            }
        }

        // Fallback to the remote IP address if X-Forwarded-For is not present or empty.
        return request.HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
    }
}
