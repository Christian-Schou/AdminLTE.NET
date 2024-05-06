using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace AdminLTE.Models;

/// <summary>
///     This is an extension model for the <see cref="IdentityUser"/> class holding data about a user in the application.
/// </summary>
/// <remarks>
///     Add profile data for application users by adding properties to the ApplicationUser class
/// </remarks>
public class ApplicationUser : IdentityUser
{
    /// <summary>
    ///     Gets or sets the firstname of the user.
    /// </summary>
    [MaxLength(90)]
    public string? FirstName { get; set; }
    
    /// <summary>
    ///     Gets or sets the lastname of the user.
    /// </summary>
    [MaxLength(90)]
    public string? LastName { get; set; }
    
    /// <summary>
    ///     Gets or sets a URL for holding the avatar URL of the user.
    /// </summary>
    [Url]
    [MaxLength(256)]
    public string? AvatarURL { get; set; }
    
    /// <summary>
    ///     Gets or sets a date for when was the user added in the application.
    /// </summary>
    public string DateRegistered { get; set; }
    
    /// <summary>
    ///     Gets or sets the position of the user.
    /// </summary>
    public string Position { get; set; }
    
    /// <summary>
    ///     Gets or sets the nickname of the user.
    /// </summary>
    public string NickName { get; set; }
}