using AdminLTE.TWC.Core.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace AdminLTE.TWC.Core.Application.Common.Behaviors
{
    /// <summary>
    ///     Represents a behavior for measuring the performance of request handling in the application.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<TRequest> _logger;
        private readonly IUser _user;
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PerformanceBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        public PerformanceBehavior(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
        {
            _timer = new Stopwatch();
            _logger = logger;
            _user = user;
            _identityService = identityService;
        }

        /// <summary>
        ///     Handles the request and measures its performance.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="next">The next handler delegate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > 500) 
            {
                var requestName = typeof(TRequest).Name;
                var userId = _user.Id ?? string.Empty;
                var userName = string.Empty;

                if (!string.IsNullOrEmpty(userId))
                {
                    userName = await _identityService.GetUserNameAsync(userId);
                }

                _logger.LogWarning("AdminLTE.NET - Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@UserId} {@UserName} {@Request}",
                    requestName, elapsedMilliseconds, userId, userName, request);
            }

            return response;
        }
    }
}
