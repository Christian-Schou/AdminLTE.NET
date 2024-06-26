﻿using ValidationException = AdminLTE.TWC.Core.Application.Common.Exceptions.ValidationException;

namespace AdminLTE.TWC.Core.Application.Common.Behaviors
{
    /// <summary>
    ///     Represents a behavior for validating requests in the application.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="validators">The validators for the request.</param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        ///     Handles the request and performs validation.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="next">The next handler delegate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var validationResults = await Task.WhenAll(
                    _validators.Select(v =>
                        v.ValidateAsync(context, cancellationToken)));

                var failures = validationResults
                    .Where(r => r.Errors.Any())
                    .SelectMany(r => r.Errors)
                    .ToList();

                if (failures.Any())
                    throw new ValidationException(failures);
            }
            return await next();
        }
    }
}
