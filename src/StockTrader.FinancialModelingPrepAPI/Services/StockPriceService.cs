using StockTrader.Domain.Exceptions;
using StockTrader.Domain.Services;
using StockTrader.FinancialModelingPrepAPI.Results;
using System.Threading.Tasks;

namespace StockTrader.FinancialModelingPrepAPI.Services
{
    public class StockPriceService : IStockPriceService
    {
        private readonly FinancialModelingPrepHttpClient _client;

        public StockPriceService(FinancialModelingPrepHttpClient client)
        {
            _client = client;
        }

        public async Task<double> GetPrice(string symbol)
        {
            string uri = "stock/real-time-price/" + symbol;

            StockPriceResult stockPriceResult = await _client.GetAsync<StockPriceResult>(uri);

            return stockPriceResult.Price == 0 ? throw new InvalidSymbolException(symbol) : stockPriceResult.Price;
        }
    }
}