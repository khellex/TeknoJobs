using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknoJobs.Domain.Entities;

namespace TeknoJobs.Domain.Interfaces
{
    /// <summary>
    /// Defines a repository for managing <see cref="Locations"/> entities, including retrieval and updates.    
    /// </summary>
    /// <remarks>This interface extends <see cref="IRepository{T}"/> to provide additional functionality
    /// specific to <see cref="Locations"/> entities. Implementations of this interface are responsible for handling
    /// data persistence and updates for <see cref="Locations"/>.</remarks>
    public interface ILocationsRepository : IRepository<Locations>
    {
        void Update(Locations entity);
    }
}
