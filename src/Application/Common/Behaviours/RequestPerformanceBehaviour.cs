using System.Diagnostics;
using System.Text;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer;
    private readonly ILogger<TRequest> _logger;

    public RequestPerformanceBehaviour(ILogger<TRequest> logger)
    {
        _timer = Stopwatch.StartNew();
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        StringBuilder logMessage = new StringBuilder();
        _timer.Start();

        try
        {
            var response = await next();
            return response;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            _timer.Stop();
            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500)
            {
                _logger.LogWarning("Long running request: {RequestName} ({ElapsedMilliseconds}ms)", request.GetType().Name, elapsedMilliseconds);
            }
        }

    }
}