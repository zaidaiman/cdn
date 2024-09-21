using System.Text;
using Log = Helpers.Log;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var method = request.Method;
        var path = request.Path + request.QueryString;

        Log.Request(_logger, context?.GetEndpoint()?.DisplayName ?? "", $"{method} {path}");

        if (method != "GET")
        {
            context!.Request.EnableBuffering();
            using var reader = new StreamReader(request.Body, Encoding.UTF8, false, 1024, leaveOpen: true);
            var body = await reader.ReadToEndAsync();
            request.Body.Seek(0L, SeekOrigin.Begin);
            Log.Request(_logger, context?.GetEndpoint()?.DisplayName ?? "", body);
        }

        await _next(context!);
    }
}
