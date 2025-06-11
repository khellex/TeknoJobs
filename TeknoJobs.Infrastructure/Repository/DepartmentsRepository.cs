using TeknoJobs.Domain.Entities;
using TeknoJobs.Domain.Interfaces;
using TeknoJobs.Infrastructure.Data;

namespace TeknoJobs.Infrastructure.Repository
{
    public class DepartmentsRepository : Repository<Departments>, IDepartmentsRepository
    {
        private readonly ApplicationDbContext _db;
        public DepartmentsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Departments entity)
        {
            _db.Departments.Update(entity);
        }
    }
}
