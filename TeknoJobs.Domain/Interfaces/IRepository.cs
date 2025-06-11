using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TeknoJobs.Domain.Interfaces
{
    /// <summary>
    /// Defines a generic repository interface for performing CRUD operations and querying entities.
    /// </summary>
    /// <remarks>This interface provides asynchronous methods for retrieving, adding, and querying entities.
    /// It supports filtering, eager loading of related entities, and tracking options.</remarks>
    /// <typeparam name="T">The type of entity managed by the repository. Must be a reference type.</typeparam>
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null);
        Task AddAsync(T entity);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
    }
}
