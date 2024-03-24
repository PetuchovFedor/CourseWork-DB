using System.Security.Cryptography;
using System.Text;

namespace ELibrary.src.ELibrary.Api.Hashing
{
    public static class Hashing
    {
        public static string Hash(string password)
        {
            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
