using Moq;
using StockTrader.Domain.Exceptions;
using StockTrader.Domain.Services;
using StockTrader.FinancialModelingPrepAPI;
using StockTrader.FinancialModelingPrepAPI.Models;
using StockTrader.FinancialModelingPrepAPI.Results;
using StockTrader.FinancialModelingPrepAPI.Services;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace StockTrader.Domain.Tests.Services
{
    public class StockPriceServiceTests
    {
        private readonly Mock<FinancialModelingPrepHttpClient> _mockClient =
            new(new HttpClient(), new FinancialModelingPrepAPIKey(string.Empty));

        [Fact]
        public async Task GetPrice_WithInvalidSymbol_ReturnsInvalidSymbolException()
        {
            string symbol = "TSLA";

            _mockClient.Setup(moq => moq.GetAsync<StockPriceResult>(It.IsAny<string>()))
                .ReturnsAsync(new StockPriceResult()
                {
                    Price = 0
                });

            InvalidSymbolException exception = await Assert.ThrowsAsync<InvalidSymbolException>(async () =>
                await StockPriceService.GetPrice(symbol));

            Assert.Equal(symbol, exception.Symbol);
        }

        [Fact]
        public async Task GetPrice_WithValidSymbol_ReturnsCorrectPrice()
        {
            double expectedPrice = 100;

            _mockClient.Setup(moq => moq.GetAsync<StockPriceResult>(It.IsAny<string>()))
                .ReturnsAsync(new StockPriceResult()
                {
                    Price = expectedPrice
                });

            double price = await StockPriceService.GetPrice(It.IsAny<string>());

            Assert.Equal(expectedPrice, price);
        }

        private IStockPriceService StockPriceService => new StockPriceService(_mockClient.Object);
    }
}