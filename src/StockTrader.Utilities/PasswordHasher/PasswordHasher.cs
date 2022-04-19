using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace StockTrader.Utilities.PasswordHasher
{
    /// <summary>
    /// Password hasher
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PasswordHasher()
        { }

        /// <summary>
        /// Creates SHA-256 has from string
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Hash value</returns>
        public string SHA256Hash(string value)
        {
            using SHA256 hash = SHA256.Create();
            return string.Concat(hash
              .ComputeHash(Encoding.UTF8.GetBytes(value))
              .Select(item => item.ToString("x2")));
        }
    }
}