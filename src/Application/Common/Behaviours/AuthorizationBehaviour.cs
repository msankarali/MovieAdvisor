using Application.Common.Exceptions;
using MediatR;

namespace Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        //TODO: user validation operations will be here
        
        // throw new ForbiddenAccessException();

        return await next();
    }
}
