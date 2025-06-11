using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Domain.Interfaces
{
    /// <summary>
    /// Defines a repository interface for managing <see cref="Jobs"/> entities, including operations for updating and
    /// generating job codes.
    /// </summary>
    /// <remarks>This interface extends <see cref="IRepository{T}"/> to provide additional functionality
    /// specific to <see cref="Jobs"/> entities. Implementations of this interface should ensure thread safety for
    /// asynchronous operations.</remarks>
    public interface IJobsRepository : IRepository<Jobs>
    {
        void Update(Jobs entity);

        Task<string> GenerateJobCodeAsync();
    }
}
