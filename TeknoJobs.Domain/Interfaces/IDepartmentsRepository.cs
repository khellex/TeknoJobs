using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Domain.Interfaces
{
    /// <summary>
    /// Defines a repository interface for managing <see cref="Departments"/> entities.
    /// </summary>
    /// <remarks>This interface extends <see cref="IRepository{T}"/> to provide additional functionality
    /// specific to  <see cref="Departments"/> entities, such as the ability to update existing records.</remarks>
    public interface IDepartmentsRepository : IRepository<Departments>
    {
        void Update(Departments entity);
    }
}
