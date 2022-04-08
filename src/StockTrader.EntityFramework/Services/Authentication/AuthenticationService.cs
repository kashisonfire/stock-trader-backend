using StockTrader.Domain.Exceptions;
using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using StockTrader.Domain.Services.Authentication;
using StockTrader.Logger;
using StockTrader.Utilities.PasswordHasher;
using System;
using System.Security;
using System.Threading.Tasks;

namespace StockTrader.EntityFramework.Services.Authentication
{
    /// <summary>
    /// Authentication service used to login or register users
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ILogger _logger;
        private readonly IAccountService _accountService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(ILogger logger, IAccountService accountService, IPasswordHasher passwordHasher)
        {
            _logger = logger;
            _accountService = accountService;
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Get an account for a user's credentials.
        /// </summary>
        /// <param name="usernameOrEmail">The username or email.</param>
        /// <param name="password">The user's password.</param>
        /// <returns>The account for the user.</returns>
        /// <exception cref="UserNotFoundException">Thrown if the user does not exist.</exception>
        /// <exception cref="InvalidPasswordException">Thrown if the password is invalid.</exception>
        /// <exception cref="Exception">Thrown if the login fails.</exception>
        public async Task<User> Login(string usernameOrEmail, SecureString password)
        {
            Account storedAccount = await _accountService.Get(e => e.AccountHolder.Username == usernameOrEmail || e.AccountHolder.Email == usernameOrEmail);

            if (storedAccount == null)
            {
                _logger.Error("Unable to find stored account", new UserNotFoundException(usernameOrEmail));
                return null;
            }

            bool passwordResult = storedAccount.AccountHolder.PasswordHash == _passwordHasher.SHA256Hash(storedAccount.AccountHolder.Username + _passwordHasher.SecureStringToString(password));

            if (passwordResult)
            {
                _logger.Info($"Username: {storedAccount.AccountHolder.Username} has logged in.");
            }

            return passwordResult ? storedAccount.AccountHolder : null;
        }

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
        public async Task<RegistrationResult> Register(string email, string username, string password, string confirmPassword, AccessLevel accessLevel)
        {
            if (password != confirmPassword)
            {
                return RegistrationResult.PasswordsDoNotMatch;
            }
            if (await _accountService.Get(e => e.AccountHolder.Email == email) != null)
            {
                return RegistrationResult.EmailAlreadyExists;
            }
            if (await _accountService.Get(e => e.AccountHolder.Username == username) != null)
            {
                return RegistrationResult.UsernameAlreadyExists;
            }

            string hashedPassword = _passwordHasher.SHA256Hash(username + password);

            User user = new()
            {
                Email = email,
                Username = username,
                PasswordHash = hashedPassword,
                DateJoined = DateTime.Now,
                AccessLevel = accessLevel
            };

            Account account = new()
            {
                AccountHolder = user,
                Balance = 500
            };

            return await _accountService.Add(account) != null ? RegistrationResult.Success : RegistrationResult.Fail;
        }

        /// <summary>
        /// Checks if the username is in our collection.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>True if valid</returns>
        public async Task<bool> ValidUser(string username)
        {
            return await _accountService.Get(e => e.AccountHolder.Username == username) != null;
        }
    }
}