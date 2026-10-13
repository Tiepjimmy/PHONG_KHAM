using System.Security.Cryptography;
using System.Text;

namespace ClinicManagement.Model.Helper
{
    public static class EncriptFunctionsHelper
    {
        public static string GenerateMd5String(string strChange)
        {
#pragma warning disable SYSLIB0021 // Type or member is obsolete
            MD5CryptoServiceProvider md5 = new();
#pragma warning restore SYSLIB0021 // Type or member is obsolete
            md5.ComputeHash(Encoding.UTF8.GetBytes(strChange));
            byte[]? encodedBytes = md5.Hash;
#pragma warning disable CS8604 // Possible null reference argument.
            string encodedPassword = BitConverter.ToString(encodedBytes);
#pragma warning restore CS8604 // Possible null reference argument.
            return encodedPassword;
        }

        public static bool VerifyMd5String(string str, string hash)
        {
            var hashinput = GenerateMd5String(str);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return 0 == comparer.Compare(hashinput, hash);
        }

        public static string GeneratePassword(string password)
        {
            return GenerateMd5String(password);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            return VerifyMd5String(password, hash);
        }
    }
}
