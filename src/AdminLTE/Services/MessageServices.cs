namespace AdminLTE.Services;

/// <summary>
///     A service implementing both the email and sms interfaces.
/// </summary>
public class AuthMessageSender : IEmailSender, ISmsSender
{
    /// <inheritdoc />
    public Task SendEmailAsync(string address, string subject, string message)
    {
        // Plug in your email service here to send an email.
        return Task.FromResult(0);
    }

    /// <inheritdoc />
    public Task SendSmsAsync(string number, string message)
    {
        // Plug in your SMS service here to send a text message.
        return Task.FromResult(0);
    }
}