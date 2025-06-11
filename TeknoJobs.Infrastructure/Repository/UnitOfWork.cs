using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Interfaces;
using TeknoJobs.Infrastructure.Data;

namespace TeknoJobs.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IDepartmentsRepository Departments { get; private set; }
        public ILocationsRepository Locations { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            Departments = new DepartmentsRepository(_db);
            Locations = new LocationsRepository(_db);
        }

        public async Task SaveChangesAsync()
        {
           await _db.SaveChangesAsync();
        }
    }
}
