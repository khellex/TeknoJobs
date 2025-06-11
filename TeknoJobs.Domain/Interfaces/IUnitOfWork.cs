using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoJobs.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IDepartmentsRepository Departments { get; }
        Task SaveChangesAsync();
    }
}
