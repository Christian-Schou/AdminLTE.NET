using System.Reflection;
using AdminLTE.TWC.Core.Application.Common.Interfaces;
using AdminLTE.TWC.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminLTE.TWC.Infrastructure.Data;

/// <summary>
///     Provides an implementation of <see cref="IAppDbContext"/> for interacting with the application's database context.
/// </summary>
public class AppDbContext : IdentityDbContext<ApplicationUser>, IAppDbContext
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    ///     Configures the schema needed for the identity framework tables.
    /// </summary>
    /// <param name="builder">Provides a simple API surface for configuring a <see cref="Microsoft.EntityFrameworkCore.Metadata.IMutableModel"/> that defines the shape of your entities, the relationships between them, and how they map to the database.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}