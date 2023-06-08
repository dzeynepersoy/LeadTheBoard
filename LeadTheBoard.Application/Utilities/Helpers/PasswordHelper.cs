using System.Security.Cryptography;
using System.Text;

namespace LeadTheBoard.Application.Utilities.Helpers
{
    public static class PasswordHelper
    {
        public static string Encrypt(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }
}
