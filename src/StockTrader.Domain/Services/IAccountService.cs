using StockTrader.Domain.Models;
using System.Threading.Tasks;

namespace StockTrader.Domain.Services
{
    public interface IAccountService : IDataService<Account>
    {
        /// <summary>
        /// Gets the account by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Account</returns>
        Task<Account> GetByUsername(string username);

        /// <summary>
        /// Gets the account by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>Account</returns>
        Task<Account> GetByEmail(string email);
    }
}