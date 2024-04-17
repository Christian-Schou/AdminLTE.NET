namespace AdminLTE.TWC.Core.Domain.Common;

/// <summary>
///     Represents a base entity class that provides functionality for managing domain events.
/// </summary>
public class BaseEntity
{
    private readonly List<BaseEvent> _domainEvents = [];

    /// <summary>
    ///     Gets or sets the identifier of the entity.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets a read-only collection of domain events associated with the entity.
    /// </summary>
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    ///     Adds a domain event to the entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to add.</param>
    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    ///     Removes a domain event from the entity.
    /// </summary>
    /// <param name="domainEvent">The domain event to remove.</param>
    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    /// <summary>
    ///     Clears all domain events associated with the entity.
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}