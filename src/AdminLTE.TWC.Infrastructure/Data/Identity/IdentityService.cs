using AdminLTE.TWC.Core.Application.Common.Interfaces;
using AdminLTE.TWC.Core.Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AdminLTE.TWC.Infrastructure.Data.Identity;

/// <summary>
///     Provides an implementation of <see cref="IIdentityService"/> for managing users in the identity system.
/// </summary>
public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="IdentityService"/> class.
    /// </summary>
    /// <param name="userManager">The user manager.</param>
    /// <param name="userClaimsPrincipalFactory">The user claims principal factory.</param>
    /// <param name="authorizationService">The authorization service.</param>
    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
    }

    /// <inheritdoc />
    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user?.UserName;
    }

    /// <inheritdoc />
    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    /// <inheritdoc />
    public async Task<bool> AuthorizeAsync(string userId, string policyName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return false;
        }

        var principal = await _userClaimsPrincipalFactory.CreateAsync(user);
        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;
    }

    /// <inheritdoc />
    public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
    {
        var user = new ApplicationUser
        {
            UserName = userName,
            Email = userName
        };

        var result = await _userManager.CreateAsync(user, password);

        return (result.ToApplicationResult(), user.Id);
    }

    /// <inheritdoc />
    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    /// <summary>
    ///     Delete a <see cref="ApplicationUser"/> from the system.
    /// </summary>
    /// <param name="user">The <see cref="ApplicationUser"/> to delete from the system.</param>
    /// <returns>The result (<see cref="IdentityResult"/>) of the delete operation as <see cref="Result"/>.</returns>
    private async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);
        return result.ToApplicationResult();
    }
}