namespace AdminLTE.TWC.Core.Domain.Common;

/// <summary>
///     Represents a base auditable entity class that adds auditing properties to a BaseEntity.
/// </summary>
public class BaseAuditableEntity : BaseEntity
{
    /// <summary>
    ///     Gets or sets the date and time when the entity was created.
    /// </summary>
    public DateTimeOffset Created { get; set; }

    /// <summary>
    ///     Gets or sets the name of the user who created the entity.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    ///     Gets or sets the date and time when the entity was last modified.
    /// </summary>
    public DateTimeOffset LastModified { get; set; }

    /// <summary>
    ///     Gets or sets the name of the user who last modified the entity.
    /// </summary>
    public string? LastModifiedBy { get; set; }
}