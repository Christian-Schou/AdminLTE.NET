using AdminLTE.TWC.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdminLTE.TWC.Infrastructure.Data.Interceptors;

/// <summary>
///     Provides an implementation of <see cref="SaveChangesInterceptor"/> for dispatching domain events.
/// </summary>
public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IMediator _mediator;

    /// <summary>
    ///     Initializes a new instance of the <see cref="DispatchDomainEventsInterceptor"/> class.
    /// </summary>
    /// <param name="mediator">The mediator.</param>
    public DispatchDomainEventsInterceptor(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    ///     Dispatches the domain events before saving changes.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="result">The result of the operation.</param>
    /// <returns>The result of the operation.</returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    ///     Asynchronously dispatches the domain events before saving changes.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="result">The result of the operation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the operation.</returns>
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    ///     Dispatches the domain events in the context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public async Task DispatchDomainEvents(DbContext? context)
    {
        if (context == null) return;

        // Get entities that have domain events
        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(entry => entry.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity);

        // Get all domain events
        var domainEvents = entities
            .SelectMany(entity => entity.DomainEvents)
            .ToList();
        
        // Clear domain events from entities
        entities.ToList().ForEach(entity => entity.ClearDomainEvents());

        // Dispatch each domain event
        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent);
    }
}