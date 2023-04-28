using System.Diagnostics;
using System.Text;

namespace WebApi.Middlewares;

public class RequestResponseLoggerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public RequestResponseLoggerMiddleware(RequestDelegate next, ILogger<RequestResponseLoggerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public static async Task<string> GetRequestBodyAsync(HttpRequest request)
    {
        request.EnableBuffering();
        using var ms = new MemoryStream();
        await request.Body.CopyToAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
        var requestBody = Encoding.UTF8.GetString(ms.ToArray());
        request.Body.Position = 0;
        return requestBody;
    }

    public static async Task<string> GetResponseBodyAsync(HttpResponse response)
    {
        response.Body.Seek(0, SeekOrigin.Begin);
        using var ms = new MemoryStream();
        await response.Body.CopyToAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);
        var responseBody = Encoding.UTF8.GetString(ms.ToArray());
        response.Body.Seek(0, SeekOrigin.Begin);
        return responseBody;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var originalRequestBody = context.Request.Body;
        var originalResponseBody = context.Response.Body;

        try
        {
            using var requestBodyStream = new MemoryStream();
            await context.Request.Body.CopyToAsync(requestBodyStream);

            requestBodyStream.Seek(0, SeekOrigin.Begin);
            var requestBodyText = await new StreamReader(requestBodyStream).ReadToEndAsync();
            requestBodyStream.Seek(0, SeekOrigin.Begin);

            context.Request.Body = requestBodyStream;
            _logger.LogInformation("Incoming request {Method} {Path} {Query} {RequestBody}",
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                requestBodyText);

            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            await _next(context);

            responseBodyStream.Seek(0, SeekOrigin.Begin);
            var responseBodyText = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);

            await responseBodyStream.CopyToAsync(originalResponseBody);

            _logger.LogInformation("Outgoing response {StatusCode} {Method} {Path} {Query} {ResponseBody} {ResponseTime:0.000}ms",
                context.Response.StatusCode,
                context.Request.Method,
                context.Request.Path,
                context.Request.QueryString,
                responseBodyText,
                sw.Elapsed.TotalMilliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request {Method} {Path} {Query}", context.Request.Method, context.Request.Path, context.Request.QueryString);
        }
        finally
        {
            context.Response.Body = originalResponseBody;
        }
    }

}
