using AdminLTE.TWC.Core.Application.Common.Interfaces;
using AdminLTE.TWC.Core.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdminLTE.TWC.Infrastructure.Data.Interceptors;

/// <summary>
///     Provides an implementation of <see cref="SaveChangesInterceptor"/> for auditing entities.
/// </summary>
public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    private readonly IUser _user;
    private readonly TimeProvider _dateTime;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AuditableEntityInterceptor"/> class.
    /// </summary>
    /// <param name="user">The current user.</param>
    /// <param name="dateTime">The time provider.</param>
    public AuditableEntityInterceptor(IUser user, TimeProvider dateTime)
    {
        _user = user;
        _dateTime = dateTime;
    }

    /// <summary>
    ///     Updates the entities before saving changes.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="result">The result of the operation.</param>
    /// <returns>The result of the operation.</returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    ///     Asynchronously updates the entities before saving changes.
    /// </summary>
    /// <param name="eventData">The event data.</param>
    /// <param name="result">The result of the operation.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the operation.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    ///     Updates the entities in the context.
    /// </summary>
    /// <param name="context">The database context.</param>
    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                var utcNow = _dateTime.GetUtcNow();
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _user.Id;
                    entry.Entity.Created = utcNow;
                }

                // LastModified and LastModifiedBy should always be updated,
                // whether the entity is being added or modified
                entry.Entity.LastModifiedBy = _user.Id;
                entry.Entity.LastModified = utcNow;
            }
        }
    }
}

/// <summary>
///     Provides extension methods for <see cref="EntityEntry"/>.
/// </summary>
public static class Extensions
{
    /// <summary>
    ///     Determines whether the entity has changed owned entities.
    /// </summary>
    /// <param name="entry">The entity entry.</param>
    /// <returns><c>true</c> if the entity has changed owned entities; otherwise, <c>false</c>.</returns>
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(reference =>
            reference.TargetEntry != null &&
            reference.TargetEntry.Metadata.IsOwned() &&
            reference.TargetEntry.State is EntityState.Added or EntityState.Modified);
}