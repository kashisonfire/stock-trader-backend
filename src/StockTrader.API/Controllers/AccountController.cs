using Microsoft.AspNetCore.Mvc;
using StockTrader.API.Authentication;
using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using StockTrader.Logger;
using System.Threading.Tasks;

namespace StockTrader.API.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IAuthenticator _authenticator;
        private readonly IAccountService _accountService;

        public AccountController(ILogger logger, IAuthenticator authenticator, IAccountService accountService)
        {
            _logger = logger;
            _authenticator = authenticator;
            _accountService = accountService;
        }

        [HttpGet("account/id/{id}")]
        public async Task<Account> GetAccountByIdAsync(int id)
        {
            return await _accountService.Get(id);
        }

        [HttpGet("account/username/{username}")]
        public async Task<Account> GetAccountByUsernameAsync(string username)
        {
            return await _accountService.GetByUsername(username);
        }

        [HttpGet("account/email/{email}")]
        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _accountService.GetByEmail(email);
        }

        [HttpPost("account/create")]
        public async Task<RegistrationResult> CreateAccountAsync(string email, string username, string password, string confirmPassword, AccessLevel accessLevel)
        {
            RegistrationResult result = await _authenticator.Register(email, username, password, confirmPassword, accessLevel);

            if (result != RegistrationResult.Success)
            {
                _logger.Error($"Unable to register user {result}");
            }

            return result;
        }
    }
}