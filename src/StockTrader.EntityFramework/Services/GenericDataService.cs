using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StockTrader.EntityFramework.Services
{
    /// <summary>
    /// Data service for all objects that inherit <see cref="DatabaseObject"/>
    /// </summary>
    /// <typeparam name="T">Generic database object</typeparam>
    public class GenericDataService<T> : IDataService<T> where T : DatabaseObject
    {
        /// <summary>
        /// Context factory for DI
        /// </summary>
        private readonly IDbContextFactory<StockTraderDbContext> _contextFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contextFactory">Context factory used</param>
        public GenericDataService(IDbContextFactory<StockTraderDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        /// <summary>
        /// Gets all entries for the entity
        /// Can be a long operation, so cancellation token introduced
        /// </summary>
        /// <param name="predicate">Expression predicate to determine match</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Entries</returns>
        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>> predicate = null,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            using (StockTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return predicate == null ?
                    await context.Set<T>().Where(predicate).ToArrayAsync(cancellationToken) :
                    await context.Set<T>().ToArrayAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Gets the entry based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Entry</returns>
        public async Task<T> Get(int id)
        {
            using (StockTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);
            }
        }

        /// <summary>
        /// Gets the entry based on the predicate
        /// </summary>
        /// <param name="predicate">Expression predicate to determine match</param>
        /// <returns>
        /// First match to predicate or null if not found
        /// </returns>
        public async Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            using (StockTraderDbContext context = _contextFactory.CreateDbContext())
            {
                return await context.Set<T>().FirstOrDefaultAsync(predicate);
            }
        }

        /// <summary>
        /// Creates a new entry
        /// </summary>
        /// <param name="entity">Entry to add</param>
        /// <returns>Created Entity</returns>
        public async Task<T> Add(T entity)
        {
            using (StockTraderDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> createdResult = await context.Set<T>().AddAsync(entity);
                int addedEntries = await context.SaveChangesAsync();

                return addedEntries == 1 ? default(T) : createdResult.Entity;
            }
        }

        /// <summary>
        /// Creates new entries
        /// </summary>
        /// <param name="entities">Entries to add</param>
        /// <returns>Created Entities</returns>
        public async Task<bool> AddRange(IEnumerable<T> entities)
        {
            using (StockTraderDbContext context = _contextFactory.CreateDbContext())
            {
                await context.Set<T>().AddRangeAsync(entities);
                int addedEntities = await context.SaveChangesAsync();

                return addedEntities == entities.Count();
            }
        }

        /// <summary>
        /// Updates entry based on Id
        /// </summary>
        /// <param name="id">Id of the entry</param>
        /// <param name="entity">Entry to add</param>
        /// <returns>Updated Entity</returns>
        public async Task<T> Update(int id, T entity)
        {
            using (StockTraderDbContext context = _contextFactory.CreateDbContext())
            {
                entity.Id = id;

                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
        }

        /// <summary>
        /// Removes entry from list based on Id
        /// </summary>
        /// <param name="id">Id of the entry</param>
        /// <returns>True on success</returns>
        public async Task<bool> Delete(int id)
        {
            using (StockTraderDbContext context = _contextFactory.CreateDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.Id == id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        /// <summary>
        /// Removes entry from list based on predicate matching
        /// </summary>
        /// <param name="predicate">Expression predicate to determine match</param>
        /// <returns>True on success</returns>
        public async Task<bool> Delete(Expression<Func<T, bool>> predicate)
        {
            using (StockTraderDbContext context = _contextFactory.CreateDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync(predicate);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }
    }

}
