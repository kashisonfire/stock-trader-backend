using Newtonsoft.Json;
using StockTrader.FinancialModelingPrepAPI.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockTrader.FinancialModelingPrepAPI
{
    public class FinancialModelingPrepHttpClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public FinancialModelingPrepHttpClient(HttpClient client, FinancialModelingPrepAPIKey apiKey)
        {
            _client = client;
            _apiKey = apiKey.Key;
        }

        public virtual async Task<T> GetAsync<T>(string uri)
        {
            HttpResponseMessage response = await _client.GetAsync($"{uri}?apikey={_apiKey}");
            string jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(jsonResponse);
        }
    }
}