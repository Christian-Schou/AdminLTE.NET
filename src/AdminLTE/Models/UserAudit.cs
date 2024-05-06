using System.ComponentModel.DataAnnotations;

namespace AdminLTE.Models;

/// <summary>
///     A model representing the properties available when a user is audited in the application.
/// </summary>
public class UserAudit
{
    /// <summary>
    ///     Gets the ID of the user audit.
    /// </summary>
    [Key] public int UserAuditId { get; }

    /// <summary>
    ///     Gets or privately sets the user id for the audit.
    /// </summary>
    [Required] public string UserId { get; private set; }

    /// <summary>
    ///     Gets or privately sets the time for when the audit was performed.
    /// </summary>
    [Required] public DateTimeOffset Timestamp { get; private set; } = DateTime.UtcNow;

    /// <summary>
    ///     Gets or sets the type of the audit event.
    /// </summary>
    [Required] public UserAuditEventType AuditEvent { get; set; }

    /// <summary>
    ///     Gets or privately sets the IP-Address of the user being audited.
    /// </summary>
    public string IpAddress { get; private set; }

    /// <summary>
    ///     Create a new user audit event.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="auditEventType">Audit Event Type.</param>
    /// <param name="ipAddress">IP-Address of the user being audited.</param>
    /// <returns></returns>
    public static UserAudit CreateAuditEvent(string userId, UserAuditEventType auditEventType, string ipAddress)
    {
        return new UserAudit { UserId = userId, AuditEvent = auditEventType, IpAddress = ipAddress };
    }
}

/// <summary>
///     The event type of the audit.
/// </summary>
public enum UserAuditEventType
{
    /// <summary>
    ///     A login audit event.
    /// </summary>
    Login = 1,
    
    /// <summary>
    ///     A failed login audit event.
    /// </summary>
    FailedLogin = 2,
    
    /// <summary>
    ///     A logout audit event.
    /// </summary>
    Logout = 3
}