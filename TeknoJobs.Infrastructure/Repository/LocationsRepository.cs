using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Entities;
using TeknoJobs.Domain.Interfaces;
using TeknoJobs.Infrastructure.Data;

namespace TeknoJobs.Infrastructure.Repository
{
    public class LocationsRepository : Repository<Locations>, ILocationsRepository
    {
        private readonly ApplicationDbContext _db;
        public LocationsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Locations entity)
        {
            _db.Locations.Update(entity);
        }
    }
}
