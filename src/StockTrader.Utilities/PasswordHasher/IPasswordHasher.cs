using System.Security;

namespace StockTrader.Utilities.PasswordHasher
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Creates SHA-256 has from string
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Hash value</returns>
        string SHA256Hash(string value);
    }
}