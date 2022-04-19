using Moq;
using StockTrader.Domain.Exceptions;
using StockTrader.Domain.Models;
using StockTrader.Domain.Services;
using StockTrader.Domain.Services.Transaction;
using StockTrader.EntityFramework.Services.TransactionServices;
using System;
using System.Threading.Tasks;
using Xunit;

namespace StockTrader.Domain.Tests.Services.Transaction
{
    public class SellStockServiceTests
    {
        private readonly Mock<IStockPriceService> _mockStockPriceService = new();
        private readonly Mock<IDataService<Account>> _mockAccountDataService = new();

        [Fact]
        public async Task SellStock_WithTooManyShares_ReturnsInsufficientSharesException()
        {
            Account seller = Helpers.CreateAccount(10);
            string symbol = "TSLA";
            int sharesOwned = 1;
            int sharesToSell = 10;
            int sharePrice = 10;
            seller.AssetTransactions.Add(Helpers.CreateAssetTransaction(seller, true, sharePrice, symbol, sharesOwned));

            InsufficientSharesException sharesException = await Assert.ThrowsAsync<InsufficientSharesException>(async () =>
                await SellStockService.SellStock(seller, symbol, sharesToSell));

            Assert.Equal(sharesToSell, sharesException.RequiredShares);
            Assert.Equal(sharesOwned, sharesException.AccountShares);
            Assert.Equal(symbol, sharesException.Symbol);
        }

        [Fact]
        public async Task SellStock_WithSuccessfulSell_ReturnsCorrectBalance()
        {
            string symbol = "TSLA";
            int stockPrice = 100;
            int sharesOwned = 10;
            int currentBalance = 100;
            Account seller = Helpers.CreateAccount(currentBalance);
            seller.AssetTransactions.Add(Helpers.CreateAssetTransaction(seller, true, stockPrice, symbol, sharesOwned));

            _mockStockPriceService.Setup(moq => moq.GetPrice(It.IsAny<string>()))
                .ReturnsAsync(stockPrice);

            Account returnedSeller = await SellStockService.SellStock(seller, symbol, sharesOwned);

            Assert.Equal(currentBalance + stockPrice * sharesOwned, returnedSeller.Balance);
        }

        private ISellStockService SellStockService => new SellStockService(_mockStockPriceService.Object, _mockAccountDataService.Object);
    }
}