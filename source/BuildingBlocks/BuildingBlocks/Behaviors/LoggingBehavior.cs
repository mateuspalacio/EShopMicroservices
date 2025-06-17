using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling request of type {RequestType} with content: {@Request}",
            typeof(TRequest).Name, request);

        var time = new Stopwatch();
        time.Start();
        var response = await next(cancellationToken);

        time.Stop();
        var timeTaken = time.Elapsed;
        if (timeTaken.TotalSeconds > 3)
            logger.LogWarning("Request of type {RequestType} took {ElapsedMilliseconds} ms to complete",
                typeof(TRequest).Name, timeTaken.TotalMilliseconds);

        logger.LogInformation("Handled request of type {RequestType} with response: {@Response}",
            typeof(TRequest).Name, response);

        return response;
    }
}