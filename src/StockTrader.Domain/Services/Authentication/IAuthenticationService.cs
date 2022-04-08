using StockTrader.Domain.Models;
using System;
using System.Security;
using System.Threading.Tasks;

namespace StockTrader.Domain.Services.Authentication
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="username">The user's name.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="confirmPassword">The user's confirmed password.</param>
        /// <param name="accessLevel">The user's access level</param>
        /// <returns>The result of the registration.</returns>
        /// <exception cref="Exception">Thrown if the registration fails.</exception>
        Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, AccessLevel accessLevel);

        /// <summary>
        /// Get an account for a user's credentials.
        /// </summary>
        /// <param name="username">The user's name.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The account for the user.</returns>
        /// <exception cref="UserNotFoundException">Thrown if the user does not exist.</exception>
        /// <exception cref="InvalidPasswordException">Thrown if the password is invalid.</exception>
        /// <exception cref="Exception">Thrown if the login fails.</exception>
        Task<User> Login(string username, SecureString password);

        /// <summary>
        /// Checks if the username is in our collection.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>True if valid</returns>
        Task<bool> ValidUser(string username);
    }
}