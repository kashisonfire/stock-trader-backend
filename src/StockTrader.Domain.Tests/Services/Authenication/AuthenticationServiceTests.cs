using Moq;
using StockTrader.Domain.Exceptions;
using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using StockTrader.Domain.Services.Authentication;
using StockTrader.EntityFramework.Services.Authentication;
using StockTrader.Logger;
using StockTrader.Utilities.PasswordHasher;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace StockTrader.Domain.Tests.Services.Authentication
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IAccountService> _mockAccountService = new();
        private readonly Mock<ILogger> _mockLogger = new();
        private readonly Mock<IPasswordHasher> _mockPasswordHasher = new();

        [Fact]
        public async Task Login_WithCorrectPasswordForExistingUsername_ReturnsAccountForCorrectUsername()
        {
            // Arrange
            string expectedUsername = "testuser";
            string password = "testpassword";

            _mockAccountService.Setup(moq => moq.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(new Account() { AccountHolder = new User() { Username = expectedUsername, PasswordHash = password } });
            _mockPasswordHasher.Setup(moq => moq.SHA256Hash(It.IsAny<string>()))
                .Returns(password);

            // Act
            Account account = await GetAuthenicationService().Login(expectedUsername, password);

            // Assert
            Assert.Equal(expectedUsername, account.AccountHolder.Username);
        }

        [Fact]
        public async Task Login_WithIncorrectPasswordForExistingUsername_ReturnsInvalidPasswordException()
        {
            // Arrange
            string expectedUsername = "testuser";
            string expectedPassword = "testPassword";

            _mockAccountService.Setup(moq => moq.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(new Account() { AccountHolder = new User() { Username = expectedUsername, PasswordHash = expectedPassword } });
            _mockPasswordHasher.Setup(moq => moq.SHA256Hash(It.IsAny<string>()))
                .Returns("incorrectPassword");

            // Act
            InvalidPasswordException exception = 
                await Assert.ThrowsAsync<InvalidPasswordException>(async () => 
                    await GetAuthenicationService().Login(expectedUsername, expectedPassword));

            // Assert
            Assert.Equal(expectedUsername, exception.UsernameOrEmail);
            Assert.Equal(expectedPassword, exception.Password);
        }

        [Fact]
        public async Task Login_WithNonRegisteredUsername_ReturnsUserNotFoundException()
        {
            // Arrange
            string expectedUsername = "testuser";
            string password = "testpassword";

            _mockAccountService.Setup(moq => moq.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(() => null);
            _mockPasswordHasher.Setup(moq => moq.SHA256Hash(It.IsAny<string>()))
                .Returns(password);

            // Act
            UserNotFoundException exception = 
                await Assert.ThrowsAsync<UserNotFoundException>(async () =>
                    await GetAuthenicationService().Login(expectedUsername, password));

            // Assert
            Assert.Equal(expectedUsername, exception.UsernameOrEmail);
        }

        [Fact]
        public async Task Register_WithCorrectFields_ReturnsSuccess()
        {
            string email = "email";
            string username = "username";
            string password = "password";

            _mockAccountService.Setup(moq => moq.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(() => null);
            _mockPasswordHasher.Setup(moq => moq.SHA256Hash(username + password))
                .Returns(password);
            _mockAccountService.Setup(moq => moq.Add(It.IsAny<Account>()))
                .ReturnsAsync(new Account());

            RegistrationResult result = await GetAuthenicationService().Register(email, username, password, password, AccessLevel.Administrator);
            Assert.Equal(RegistrationResult.Success, result);
        }

        [Fact]
        public async Task Register_WithDatabaseServerDown_ReturnsFail()
        {
            string email = "email";
            string username = "username";
            string password = "password";
            Account account = new()
            {
                AccountHolder = new()
                {
                    Email = email,
                    Username = username,
                    PasswordHash = password,
                    DateJoined = DateTime.Now,
                    AccessLevel = AccessLevel.Administrator
                },
                Balance = 500
            };

            _mockAccountService.Setup(moq => moq.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(() => null);
            _mockPasswordHasher.Setup(moq => moq.SHA256Hash(username + password))
                .Returns(password);
            _mockAccountService.Setup(moq => moq.Add(account))
                .ReturnsAsync(() => null);

            RegistrationResult result = await GetAuthenicationService().Register(email, username, password, password, AccessLevel.Administrator);
            Assert.Equal(RegistrationResult.Fail, result);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ReturnsEmailAlreadyExists()
        {
            string email = "email";
            _mockAccountService.Setup(moq => moq.Get(e => e.AccountHolder.Email == email))
                .ReturnsAsync(new Account());
            RegistrationResult result = await GetAuthenicationService().Register(email, "username", "password", "password", AccessLevel.Administrator);
            Assert.Equal(RegistrationResult.EmailAlreadyExists, result);
        }

        [Fact]
        public async Task Register_WithExistingUsername_ReturnsUsernameAlreadyExists()
        {
            string email = "email";
            string username = "username";
            _mockAccountService.Setup(moq => moq.Get(e => e.AccountHolder.Email == email))
                .ReturnsAsync(() => null);
            _mockAccountService.Setup(moq => moq.Get(e => e.AccountHolder.Username == username))
                .ReturnsAsync(new Account());
            RegistrationResult result = await GetAuthenicationService().Register(email, username, "password", "password", AccessLevel.Administrator);
            Assert.Equal(RegistrationResult.UsernameAlreadyExists, result);
        }

        [Fact]
        public async Task Register_WithNonMatchingPasswords_ReturnsPasswordsDoNotMatch()
        {
            RegistrationResult result = await GetAuthenicationService().Register("email", "username", "password", "incorrectPassword", AccessLevel.Administrator);
            Assert.Equal(RegistrationResult.PasswordsDoNotMatch, result);
        }

        [Fact]
        public async Task ValidUser_WithExistingUsername_ReturnsTrue()
        {
            _mockAccountService.Setup(moq => moq.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(new Account());
            bool result = await GetAuthenicationService().ValidUser("username");
            Assert.True(result);
        }

        [Fact]
        public async Task ValidUser_WithNonExistingUsername_ReturnsFalse()
        {
            _mockAccountService.Setup(moq => moq.Get(It.IsAny<Expression<Func<Account, bool>>>()))
                .ReturnsAsync(() => null);
            bool result = await GetAuthenicationService().ValidUser("username");
            Assert.False(result);
        }

        private IAuthenticationService GetAuthenicationService() =>
            new AuthenticationService(_mockLogger.Object, _mockAccountService.Object, _mockPasswordHasher.Object);
    }
}