using System;
using System.Security.Cryptography;
using System.Text;

namespace ClaimsDesk.API.Helpers
{
    public static class ApiKeyGenerator
    {
        public static (string AppId, string ApiSecret, string KeyHash, string KeyPrefix) GenerateNewKey()
        {
            var appIdBytes = new byte[8];
            var secretBytes = new byte[32];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(appIdBytes);
                rng.GetBytes(secretBytes);
            }

            string appId = "cd_app_" + Convert.ToHexString(appIdBytes).ToLower();
            string apiSecret = "cd_sec_" + Convert.ToBase64String(secretBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");

            string keyPrefix = apiSecret.Substring(0, 15) + "..."; // E.g., cd_sec_ABCDEF...

            string keyHash = ComputeSha256Hash(apiSecret);

            return (appId, apiSecret, keyHash, keyPrefix);
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
