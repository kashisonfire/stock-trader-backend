using Microsoft.EntityFrameworkCore;
using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace StockTrader.EntityFramework.Services
{
    public class AccountService : IAccountService
    {
        private readonly IDbContextFactory<StockTraderDbContext> _contextFactory;
        private readonly IDataService<Account> _dataService;

        public AccountService(IDbContextFactory<StockTraderDbContext> contextFactory, IDataService<Account> dataService)
        {
            _contextFactory = contextFactory;
            _dataService = dataService;
        }

        async Task<Account> IDataService<Account>.Add(Account entity)
        {
            return await _dataService.Add(entity);
        }

        async Task<bool> IDataService<Account>.AddRange(IEnumerable<Account> entities)
        {
            return await _dataService.AddRange(entities);
        }

        async Task<bool> IDataService<Account>.Delete(int id)
        {
            return await _dataService.Delete(id);
        }

        async Task<bool> IDataService<Account>.Delete(Expression<Func<Account, bool>> predicate)
        {
            return await _dataService.Delete(predicate);
        }

        async Task<Account> IDataService<Account>.Get(int id)
        {
            using StockTraderDbContext context = _contextFactory.CreateDbContext();
            return await context.Accounts
                .Include(a => a.AccountHolder)
                .Include(a => a.AssetTransactions)
                .FirstOrDefaultAsync((e) => e.Id == id);
        }

        async Task<Account> IDataService<Account>.Get(Expression<Func<Account, bool>> predicate)
        {
            using StockTraderDbContext context = _contextFactory.CreateDbContext();
            return await context.Accounts
                .Include(a => a.AccountHolder)
                .Include(a => a.AssetTransactions)
                .FirstOrDefaultAsync(predicate);
        }

        async Task<IEnumerable<Account>> IDataService<Account>.GetAll(Expression<Func<Account, bool>> predicate, CancellationToken cancellationToken)
        {
            using StockTraderDbContext context = _contextFactory.CreateDbContext();
            return predicate == null ?
                await context.Accounts
                    .Include(a => a.AccountHolder)
                    .Include(a => a.AssetTransactions)
                    .Where(predicate)
                    .ToArrayAsync(cancellationToken) :
                await context.Accounts
                    .Include(a => a.AccountHolder)
                    .Include(a => a.AssetTransactions)
                    .ToArrayAsync(cancellationToken);
        }

        async Task<Account> IAccountService.GetByEmail(string email)
        {
            using StockTraderDbContext context = _contextFactory.CreateDbContext();
            return await context.Accounts
                .Include(a => a.AccountHolder)
                .Include(a => a.AssetTransactions)
                .FirstOrDefaultAsync(a => a.AccountHolder.Email == email);
        }

        async Task<Account> IAccountService.GetByUsername(string username)
        {
            using StockTraderDbContext context = _contextFactory.CreateDbContext();
            return await context.Accounts
                .Include(a => a.AccountHolder)
                .Include(a => a.AssetTransactions)
                .FirstOrDefaultAsync(a => a.AccountHolder.Username == username);
        }

        async Task<Account> IDataService<Account>.Update(int id, Account entity)
        {
            return await _dataService.Add(entity);
        }
    }
}