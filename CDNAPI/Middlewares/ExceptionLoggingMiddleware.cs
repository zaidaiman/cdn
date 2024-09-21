using Log = Helpers.Log;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionLoggingMiddleware> _logger;

    public ExceptionLoggingMiddleware(RequestDelegate next, ILogger<ExceptionLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            Log.Error(_logger, context?.GetEndpoint()!.DisplayName ?? "", ex);
            throw; // Rethrow the exception to let the default exception handler handle it
        }
    }
}

// using System.Net;
// using System.Text;
// using System.Text.Json;
// using Log = Helpers.Log;

// public class ExceptionLoggingMiddleware : IFunctionsWorkerMiddleware
// {
//     private readonly ILogger _logger;

//     public ExceptionLoggingMiddleware(ILoggerFactory loggerFactory)
//     {
//         _logger = loggerFactory.CreateLogger<ExceptionLoggingMiddleware>();
//     }

//     public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
//     {
//         try
//         {
//             await next(context);
//         }
//         catch (Exception ex)
//         {
//             StringBuilder trace = new();
//             Exception? currentException = ex;

//             var httpReqData = await context.GetHttpRequestDataAsync();
//             if (httpReqData == null)
//             {
//                 Log.Error(_logger, "ExceptionLoggingMiddleware", ex.Message);
//                 return;
//             }

//             using var reader = new StreamReader(httpReqData.Body, Encoding.UTF8, false, 1024, leaveOpen: true);
//             var requestBody = await reader.ReadToEndAsync();
//             httpReqData.Body.Seek(0L, SeekOrigin.Begin);

//             while (currentException is not null)
//             {
//                 trace.AppendLine(currentException.Message);
//                 trace.AppendLine($"URL: {httpReqData?.Url}");
//                 trace.AppendLine($"Body: {requestBody}");
//                 currentException = currentException.InnerException;
//             }

//             string className = context.FunctionDefinition.EntryPoint;
//             Log.Error(_logger, className, JsonSerializer.Serialize(trace.ToString()));

//             //_logger.LogError(ex, "Invalid Status code for request: '{ApiUrl}', received code: {ReceivedStatusCode}");
//             //var ai = new TelemetryClient(); // to-do: add Telemetry to log exceptions in azure logs
//             //ai.TrackException(tracer);

//             var newHttpResponse = httpReqData!.CreateResponse(HttpStatusCode.InternalServerError);
//             await newHttpResponse.WriteAsJsonAsync(new
//             {
//                 success = false,
//                 //errors = JsonSerializer.Serialize(trace.ToArray()),
//                 error = "Something went wrong",
//             }, newHttpResponse.StatusCode);
//             context.GetInvocationResult().Value = newHttpResponse;
//         }
//     }
// }
