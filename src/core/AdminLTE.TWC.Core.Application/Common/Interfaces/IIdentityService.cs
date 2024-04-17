using AdminLTE.TWC.Core.Application.Common.Models;

namespace AdminLTE.TWC.Core.Application.Common.Interfaces;

/// <summary>
///     Provides methods for interacting with the identity system.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    ///     Gets the username for the specified user ID.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. When the task completes, the result contains the user name.</returns>
    Task<string?> GetUserNameAsync(string userId);

    /// <summary>
    ///     Determines whether the specified user is in the specified role.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="role">The role to check.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. When the task completes, the result is <c>true</c> if the user is in the role; otherwise, <c>false</c>.</returns>
    Task<bool> IsInRoleAsync(string userId, string role);

    /// <summary>
    ///     Determines whether the specified user is authorized for the specified policy.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="policyName">The name of the policy.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. When the task completes, the result is <c>true</c> if the user is authorized; otherwise, <c>false</c>.</returns>
    Task<bool> AuthorizeAsync(string userId, string policyName);

    /// <summary>
    ///     Creates a new user with the specified user name and password.
    /// </summary>
    /// <param name="userName">The user name of the new user.</param>
    /// <param name="password">The password of the new user.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. When the task completes, the result contains the result of the operation and the ID of the new user.</returns>
    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    /// <summary>
    ///     Deletes the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. When the task completes, the result contains the result of the operation.</returns>
    Task<Result> DeleteUserAsync(string userId);
}