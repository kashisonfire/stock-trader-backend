namespace StockTrader.FinancialModelingPrepAPI.Models
{
    /// <summary>
    /// Contains API key used for Financial Modeling Prep API
    /// </summary>
    public class FinancialModelingPrepAPIKey
    {
        public string Key { get; }

        public FinancialModelingPrepAPIKey(string key)
        {
            Key = key;
        }
    }
}