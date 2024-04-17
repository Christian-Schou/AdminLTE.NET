using AdminLTE.TWC.Core.Application.Common.Exceptions;
using AdminLTE.TWC.Core.Application.Common.Interfaces;
using AdminLTE.TWC.Core.Application.Common.Security;
using System.Reflection;

namespace AdminLTE.TWC.Core.Application.Common.Behaviors
{
    /// <summary>
    ///     Represents a behavior for performing authorization checks on requests in the application.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly IUser _user;
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AuthorizationBehavior{TRequest, TResponse}"/> class.
        /// </summary>
        /// <param name="user">The user service.</param>
        /// <param name="identityService">The identity service.</param>
        public AuthorizationBehavior(IUser user, IIdentityService identityService)
        {
            _user = user;
            _identityService = identityService;
        }

        /// <summary>
        ///     Handles the request and performs authorization checks.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="next">The next handler delegate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response.</returns>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

            if (authorizeAttributes.Any())
            {
                // Must be an authenticated user!
                if(_user.Id == null)
                    throw new UnauthorizedAccessException();

                // Doing role-based authorization
                var authorizeAttributesWithRoles = authorizeAttributes.Where(attribute => !string.IsNullOrWhiteSpace(attribute.Roles));

                if (authorizeAttributesWithRoles.Any())
                {
                    var authorized = false;

                    foreach (var roles in authorizeAttributesWithRoles.Select(attribute => attribute.Roles.Split(',')))
                    {
                        foreach(var role in roles)
                        {
                            var isInRole = await _identityService.IsInRoleAsync(_user.Id, role.Trim());
                            if (isInRole)
                            {
                                authorized = true;
                                break;
                            }
                        }
                    }

                    if (!authorized)
                    {
                        throw new ForbiddenAccessException();
                    }
                }

                // Doing Policy-based authorization
                var authorizeAttributesWithPolicies = authorizeAttributes.Where(attribute => !string.IsNullOrWhiteSpace(attribute.Policy));
                if (authorizeAttributesWithPolicies.Any())
                {
                    foreach (var policy in authorizeAttributesWithPolicies.Select(attribute => attribute.Policy))
                    {
                        var authorized = await _identityService.AuthorizeAsync(_user.Id, policy);

                        if (!authorized)
                        {
                            throw new ForbiddenAccessException();
                        }
                    }
                }
            }

            // User is authenticated / no authorization is required
            return await next();
        }
    }
}
