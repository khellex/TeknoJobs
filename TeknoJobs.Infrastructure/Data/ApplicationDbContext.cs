using Microsoft.EntityFrameworkCore;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Infrastructure.Data
{
    /// <summary>
    /// Represents the application's database context, providing access to the underlying database using Entity
    /// Framework Core.
    /// </summary>
    /// <remarks>This class is used to configure and interact with the application's database. It inherits
    /// from <see cref="DbContext"/> and provides <see cref="DbSet{TEntity}"/> properties for querying and saving
    /// instances of the specified entity types.</remarks>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<Departments> Departments { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<Jobs> Jobs { get; set; }
    }
}
