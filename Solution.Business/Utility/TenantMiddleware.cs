using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantMiddleware> _logger;

    public TenantMiddleware(RequestDelegate next, ILogger<TenantMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var tenantId = context.Session.GetString("TenantId");

        if (!string.IsNullOrEmpty(tenantId) && !IsStaticFile(context.Request.Path))
        {
            var encodedTenantId = System.Net.WebUtility.UrlEncode(tenantId);
            PrependTenantId(context, encodedTenantId);
        }

        await _next(context);
    }

    private void PrependTenantId(HttpContext context, string tenantId)
    {
        var originalPath = context.Request.Path;
        var basePath = new PathString($"/{tenantId}");

        if (!originalPath.StartsWithSegments(basePath))
        {
            var newPath = new PathString($"{basePath}{originalPath}");
            _logger.LogInformation($"Original URL: {originalPath}, Rewritten URL: {newPath}");
            context.Request.Path = newPath;
        }
    }

    private bool IsStaticFile(PathString path)
    {
        string[] staticFileExtensions = { ".css", ".js", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".ico", ".svg", ".woff", ".woff2", ".ttf", ".eot", ".mp4", ".webm", ".ogv" };
        string pathValue = path.Value.ToLowerInvariant();
        return staticFileExtensions.Any(ext => pathValue.EndsWith(ext));
    }
}
