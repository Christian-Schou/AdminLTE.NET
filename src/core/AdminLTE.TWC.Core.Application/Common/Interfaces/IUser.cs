namespace AdminLTE.TWC.Core.Application.Common.Interfaces;

/// <summary>
///     Provides an interface for a user in the application.
/// </summary>
public interface IUser
{
    /// <summary>
    ///     Gets the ID of the user.
    /// </summary>
    string? Id { get; }
}