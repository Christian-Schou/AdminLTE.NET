using AdminLTE.TWC.Core.Application.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace AdminLTE.TWC.Infrastructure.Identity;

/// <summary>
///     Provides extension methods for <see cref="IdentityResult"/>.
/// </summary>
public static class IdentityResultExtension
{
    /// <summary>
    ///     Converts an <see cref="IdentityResult"/> to a <see cref="Result"/>.
    /// </summary>
    /// <param name="result">The <see cref="IdentityResult"/> to convert.</param>
    /// <returns>A <see cref="Result"/> that represents the same operation as the <see cref="IdentityResult"/>.</returns>
    public static Result ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? Result.Success()
            : Result.Failure(result.Errors.Select(error => error.Description));
    }
}