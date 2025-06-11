using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknoJobs.Domain.Interfaces
{
    /// <summary>
    /// Represents a unit of work that encapsulates multiple repositories and provides a mechanism  to commit changes to
    /// the underlying data store.
    /// </summary>
    /// <remarks>This interface is typically used to coordinate operations across multiple repositories 
    /// within a single transaction or logical unit of work. It ensures that changes made to  the repositories are
    /// persisted together.</remarks>
    public interface IUnitOfWork
    {
        IDepartmentsRepository Departments { get; }
        ILocationsRepository Locations { get; }
        IJobsRepository Jobs { get; }
        Task SaveChangesAsync();
    }
}
