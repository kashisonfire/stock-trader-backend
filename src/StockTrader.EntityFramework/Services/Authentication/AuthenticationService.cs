using StockTrader.Domain.Exceptions;
using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using StockTrader.Domain.Services.Authentication;
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
        private readonly IDataService<User> _dataService;
        private readonly IPasswordHasher _passwordHasher;

        public AuthenticationService(IDataService<User> dataService, IPasswordHasher passwordHasher)
        {
            _dataService = dataService;
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
            User storedUser = await _dataService.Get(e => e.Username == usernameOrEmail || e.Email == usernameOrEmail);

            if (storedUser == null)
            {
                throw new UserNotFoundException(usernameOrEmail);
            }

            bool passwordResult = storedUser.PasswordHash == _passwordHasher.SHA256Hash(storedUser.Username + _passwordHasher.SecureStringToString(password));

            return passwordResult ? storedUser : null;
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
        public async Task<RegistrationResult> Register(string email, string username, SecureString password, SecureString confirmPassword, AccessLevel accessLevel)
        {
            if (password != confirmPassword)
            {
                return RegistrationResult.PasswordsDoNotMatch;
            }
            if (await _dataService.Get(e => e.Email == email) != null)
            {
                return RegistrationResult.EmailAlreadyExists;
            }
            if (await _dataService.Get(e => e.Username == username) != null)
            {
                return RegistrationResult.UsernameAlreadyExists;
            }

            string hashedPassword = _passwordHasher.SHA256Hash(username + _passwordHasher.SecureStringToString(password));

            User user = new User()
            {
                Email = email,
                Username = username,
                PasswordHash = hashedPassword,
                DateJoined = DateTime.Now,
                AccessLevel = accessLevel
            };

            await _dataService.Add(user);

            return RegistrationResult.Success;
        }

        /// <summary>
        /// Checks if the username is in our collection.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>True if valid</returns>
        public async Task<bool> ValidUser(string username)
        {
            return await _dataService.Get(e => e.Username == username) != null;
        }
    }
}