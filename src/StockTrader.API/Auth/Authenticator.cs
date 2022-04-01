using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using System;
using System.Security;
using System.Threading.Tasks;

namespace StockTrader.API.Auth
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;

        public Authenticator(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        /// <summary>
        /// Current logged in user
        /// </summary>
        public User CurrentUser { get; private set; }

        /// <summary>
        /// User currently logged in
        /// </summary>
        public bool IsUserLoggedIn => CurrentUser != null;

        /// <summary>
        /// User has logged in/out or registered
        /// </summary>
        public event Action StateChanged;

        /// <summary>
        /// Checks if the username is in our collection.
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>True if valid</returns>
        public async Task<bool> ValidUser(string username)
        {
            return await _authenticationService.ValidUser(username);
        }

        /// <summary>
        /// Login to the application.
        /// </summary>
        /// <param name="username">The user's name.</param>
        /// <param name="password">The user's password.</param>
        /// <exception cref="UserNotFoundException">Thrown if the user does not exist.</exception>
        /// <exception cref="InvalidPasswordException">Thrown if the password is invalid.</exception>
        /// <exception cref="Exception">Thrown if the login fails.</exception>
        public async Task Login(string username, SecureString password)
        {
            CurrentUser = await _authenticationService.Login(username, password);
            StateChanged?.Invoke();
        }

        /// <summary>
        /// Logs current user out
        /// </summary>
        public void Logout()
        {
            CurrentUser = null;
            StateChanged?.Invoke();
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
            return await _authenticationService.Register(email, username, password, confirmPassword, accessLevel);
        }
    }
}