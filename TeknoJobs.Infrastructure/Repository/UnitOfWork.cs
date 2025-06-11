using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Interfaces;
using TeknoJobs.Infrastructure.Data;

namespace TeknoJobs.Infrastructure.Repository
{
    /// <summary>
    /// Provides a centralized mechanism for managing repositories and saving changes to the database.
    /// </summary>
    /// <remarks>The <see cref="UnitOfWork"/> class encapsulates multiple repositories and ensures that
    /// changes  made to the database are committed as a single unit. This class is typically used in scenarios  where
    /// multiple related operations need to be performed within a single transaction.</remarks>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IDepartmentsRepository Departments { get; private set; }
        public ILocationsRepository Locations { get; private set; }
        public IJobsRepository Jobs { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            Departments = new DepartmentsRepository(_db);
            Locations = new LocationsRepository(_db);
            Jobs = new JobsRepository(_db);
        }

        public async Task SaveChangesAsync()
        {
           await _db.SaveChangesAsync();
        }
    }
}
