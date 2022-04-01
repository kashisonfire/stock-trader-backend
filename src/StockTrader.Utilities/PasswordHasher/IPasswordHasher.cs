using System.Security;

namespace StockTrader.Utilities.PasswordHasher
{
    public interface IPasswordHasher
    {
        /// <summary>
        /// Converts a secure string into an encyrpted string
        /// </summary>
        /// <param name="value">The secure string to convert</param>
        /// <returns>The converted secure string</returns>
        string SecureStringToString(SecureString value);

        /// <summary>
        /// Creates SHA-256 has from string
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Hash value</returns>
        string SHA256Hash(string value);

        /// <summary>
        /// Converts a string to a securestring.
        /// IMPORTANT! Only use for adding default users!
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns>Secure string of the password</returns>
        SecureString ConvertToSecureString(string password);
    }
}