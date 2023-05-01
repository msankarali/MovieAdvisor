using FluentValidation;
using MediatR;

namespace Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (_validators is null) return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResult = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken))
            );

        var failures = validationResult
            .Where(vr => vr.Errors.Any())
            .SelectMany(e => e.Errors)
            .ToList();

        if (failures.Any())
        {
            throw new Exceptions.ValidationException(failures);
        }

        return await next();
    }
}
