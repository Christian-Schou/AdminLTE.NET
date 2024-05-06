namespace AdminLTE.Services;

/// <summary>
///     A service for sending emails. Other Email services in the application, should inherit from this interface.
/// </summary>
public interface IEmailSender
{
    /// <summary>
    ///     Send an email using the selected provider.
    /// </summary>
    /// <param name="address">The email address.</param>
    /// <param name="subject">The email subject.</param>
    /// <param name="message">The email message/body/content.</param>
    /// <returns></returns>
    Task SendEmailAsync(string address, string subject, string message);
}