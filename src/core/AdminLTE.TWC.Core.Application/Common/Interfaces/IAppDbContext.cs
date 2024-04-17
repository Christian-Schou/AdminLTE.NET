namespace AdminLTE.TWC.Core.Application.Common.Interfaces;

/// <summary>
///     Provides methods for interacting with the application's database context.
/// </summary>
public interface IAppDbContext
{
    /// <summary>
    ///     Asynchronously saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}