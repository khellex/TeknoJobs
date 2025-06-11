using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Domain.Interfaces
{
    public interface IDepartmentsRepository : IRepository<Departments>
    {
        void Update(Departments entity);
    }
}
