using StockTrader.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StockTrader.Domain.Services
{
    /// <summary>
    /// Basic CRUD operations used for async communication to the database
    /// </summary>
    /// <typeparam name="T">Constrained to database objects only</typeparam>
    public interface IDataService<T> where T : DatabaseObject
    {
        /// <summary>
        /// Gets all entries for the entity
        /// Can be a long operation, so cancellation token introduced
        /// </summary>
        /// <param name="predicate">Expression predicate to determine match</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Entries</returns>
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the entry based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entry</returns>
        Task<T> Get(int id);

        /// <summary>
        /// Gets the entry based on the predicate
        /// </summary>
        /// <param name="predicate">Expression predicate to determine match</param>
        /// <returns>
        /// First match to predicate or null if not found
        /// </returns>
        Task<T> Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Creates a new entry
        /// </summary>
        /// <param name="entity">Entry to add</param>
        /// <returns>Created Entity</returns>
        Task<T> Add(T entity);

        /// <summary>
        /// Creates new entries
        /// </summary>
        /// <param name="entities">Entries to add</param>
        /// <returns>True on success</returns>
        Task<bool> AddRange(IEnumerable<T> entities);

        /// <summary>
        /// Updates entry based on Id
        /// </summary>
        /// <param name="id">Id of the entry</param>
        /// <param name="entity">Entry to add</param>
        /// <returns>Updated Entity</returns>
        Task<T> Update(int id, T entity);

        /// <summary>
        /// Removes entry from list based on Id
        /// </summary>
        /// <param name="id">Id of the entry</param>
        /// <returns>True on success</returns>
        Task<bool> Delete(int id);

        /// <summary>
        /// Removes entry from list based on predicate matching
        /// </summary>
        /// <param name="predicate">Expression predicate to determine match</param>
        /// <returns>True on success</returns>
        Task<bool> Delete(Expression<Func<T, bool>> predicate);
    }
}
