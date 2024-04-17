using AdminLTE.TWC.Core.Application.Common.Interfaces;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace AdminLTE.TWC.Core.Application.Common.Behaviors
{
    /// <summary>
    ///     Represents a behavior for logging requests in the application.
    /// </summary>
    /// <typeparam name="TRequest">The request.</typeparam>
    public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
    {
        private readonly ILogger<TRequest> _logger;
        private readonly IUser _user;
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LoggingBehavior{TRequest}"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="user">The user service.</param>
        /// <param name="identityService">The identity service.</param>
        public LoggingBehavior(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
        {
            _logger = logger;
            _user = user;
            _identityService = identityService;
        }

        /// <summary>
        ///     Processes the request and log it.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var userId = _user.Id ?? string.Empty;
            string? userName = string.Empty;

            if (!string.IsNullOrEmpty(userId))
            {
                userName = await _identityService.GetUserNameAsync(userId);
            }

            _logger.LogInformation("AdminLTE.NET Request: {Name} {@UserId} {@UserName} {@Request}",
                requestName, userId, userName, request);
        }
    }
}
