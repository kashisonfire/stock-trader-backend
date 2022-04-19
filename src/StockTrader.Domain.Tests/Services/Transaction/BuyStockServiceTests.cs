using Moq;
using StockTrader.Domain.Exceptions;
using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using StockTrader.Domain.Services.Transaction;
using StockTrader.EntityFramework.Services.Transaction;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace StockTrader.Domain.Tests.Services.Transaction
{
    public class BuyStockServiceTests
    {
        private readonly Mock<IStockPriceService> _mockStockPriceService = new();
        private readonly Mock<IDataService<Account>> _mockAccountDataService = new();

        [Fact]
        public async Task BuyStock_WithTransactionPriceHigherThanBuyerBalance_ReturnsInsufficientFundsException()
        {
            string symbol = "TSLA";
            int stockPrice = 100;
            int shares = 5;
            int balance = 400;
            int expectedTransactionCost = stockPrice * shares;
            Account account = Helpers.CreateAccount(400);

            _mockStockPriceService.Setup(moq => moq.GetPrice(symbol))
                .ReturnsAsync(stockPrice);

            InsufficientFundsException exception = await Assert.ThrowsAsync<InsufficientFundsException>(
                async () => await BuyStockService.BuyStock(account, symbol, shares));

            Assert.Equal(balance, exception.AccountBalance);
            Assert.Equal(expectedTransactionCost, exception.RequiredBalance);
        }

        [Fact]
        public async Task BuyStock_WithSuccessfulPurchase_ReturnsAccountWithNewTransaction()
        {
            Account account = Helpers.CreateAccount(1000);

            _mockStockPriceService.Setup(moq => moq.GetPrice(It.IsAny<string>()))
                .ReturnsAsync(10);

            Account returnedAccount = await BuyStockService.BuyStock(account, It.IsAny<string>(), 10);

            Assert.Single(returnedAccount.AssetTransactions);
        }

        [Fact]
        public async Task BuyStock_WithSuccessfulPurchase_ReturnsAccountWithNewBalance()
        {
            Account account = Helpers.CreateAccount(1000);

            _mockStockPriceService.Setup(moq => moq.GetPrice(It.IsAny<string>()))
                .ReturnsAsync(10);

            Account returnedAccount = await BuyStockService.BuyStock(account, It.IsAny<string>(), 10);

            Assert.Equal(900, returnedAccount.AssetTransactions.First().Account.Balance);
        }

        private IBuyStockService BuyStockService => new BuyStockService(_mockStockPriceService.Object, _mockAccountDataService.Object);
    }
}