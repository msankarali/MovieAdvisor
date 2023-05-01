using System.Net;
using Application.Common.Exceptions;
using Application.Common.Models;
using Domain.Exceptions;
using Newtonsoft.Json;

namespace WebApi.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            Result result;

            switch (error)
            {
                case ForbiddenAccessException ex:
                    _logger.LogCritical("");
                    result = Result.Error(ex.Message);
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                case NotFoundException ex:
                    result = Result.Error(ex.Message);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case ValidationException ex:
                    result = Result.Information(string.Join("\n", ((ValidationException)ex).Errors.SelectMany(kv => kv.Value.Select(v => $"{kv.Key}: {v}"))));
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case RatingOutOfBoundsException ex:
                    result = Result.Information(ex.Message);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case KeyNotFoundException ex:
                    result = Result.Error(ex.Message);
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case NotSupportedException ex:
                    result = Result.Error(ex.Message);
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    result = Result.Error("Undetected error! Please contact with us.");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            await response.WriteAsync(JsonConvert.SerializeObject(result));
        }
    }
}
