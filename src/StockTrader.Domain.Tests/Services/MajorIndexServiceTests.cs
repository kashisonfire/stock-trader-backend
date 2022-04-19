using Moq;
using SimpleTrader.Domain.Services;
using StockTrader.Domain.Models;
using StockTrader.FinancialModelingPrepAPI;
using StockTrader.FinancialModelingPrepAPI.Models;
using StockTrader.FinancialModelingPrepAPI.Services;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace StockTrader.Domain.Tests.Services
{
    public class MajorIndexServiceTests
    {
        private readonly Mock<FinancialModelingPrepHttpClient> _mockClient =
            new(new HttpClient(), new FinancialModelingPrepAPIKey(string.Empty));

        [Fact]
        public async Task GetMajorIndex_WithCorrectEnum_ReturnsMajorIndex()
        {
            MajorIndexType expectedMajorIndexType = MajorIndexType.Nasdaq;

            _mockClient.Setup(moq => moq.GetAsync<MajorIndex>(It.IsAny<string>()))
                .ReturnsAsync(new MajorIndex()
                {
                    Type = expectedMajorIndexType,
                });

            MajorIndex majorIndex = await MajorIndexService.GetMajorIndex(expectedMajorIndexType);

            Assert.Equal(expectedMajorIndexType, majorIndex.Type);
        }

        private IMajorIndexService MajorIndexService => new MajorIndexService(_mockClient.Object);
    }
}