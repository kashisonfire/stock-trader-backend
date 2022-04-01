using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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
        /// Converts a string to a securestring.
        /// IMPORTANT! Only use for adding default users!
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns>Secure string of the password</returns>
        public SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }

        /// <summary>
        /// Converts a secure string into an encyrpted string
        /// </summary>
        /// <param name="value">The secure string to convert</param>
        /// <returns>The converted secure string</returns>
        public string SecureStringToString(SecureString value)
        {
            IntPtr valuePtr = IntPtr.Zero;
            try
            {
                valuePtr = Marshal.SecureStringToGlobalAllocUnicode(value);
                return Marshal.PtrToStringUni(valuePtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(valuePtr);
            }
        }

        /// <summary>
        /// Creates SHA-256 has from string
        /// </summary>
        /// <param name="value">String value</param>
        /// <returns>Hash value</returns>
        public string SHA256Hash(string value)
        {
            using (SHA256 hash = SHA256.Create())
            {
                return string.Concat(hash
                  .ComputeHash(Encoding.UTF8.GetBytes(value))
                  .Select(item => item.ToString("x2")));
            }
        }
    }
}