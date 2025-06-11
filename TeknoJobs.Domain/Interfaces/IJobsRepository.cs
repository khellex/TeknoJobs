using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Domain.Interfaces
{
    public interface IJobsRepository : IRepository<Jobs>
    {
        void Update(Jobs entity);

        Task<string> GenerateJobCodeAsync();
    }
}
