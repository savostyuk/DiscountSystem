using MediatR;
using Microsoft.Extensions.Logging;

namespace DiscountSystem.Application.Common.Behavior;

public class UnhandledExceptionsBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ILogger<TRequest> _logger;

    public UnhandledExceptionsBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex) 
        { 
            var requestName = typeof(TRequest).Name;

            _logger.LogError(ex, $"Unhandled error occured for request {requestName} {request}");

            throw;
        }
    }
}
