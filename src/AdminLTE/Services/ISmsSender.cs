namespace AdminLTE.Services;

/// <summary>
///     An interface for sending text messages using the sms protocol.
/// </summary>
public interface ISmsSender
{
    /// <summary>
    ///     Send a text message using a phone number.
    /// </summary>
    /// <param name="number">Mobile phone number.</param>
    /// <param name="message">Text message/content.</param>
    /// <returns></returns>
    Task SendSmsAsync(string number, string message);
}