using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ClaimsDesk.API.Helpers
{
    public class LicenseValidationResult
    {
        public bool Valid { get; set; }
        public string? Type { get; set; }
        public int Seats { get; set; }
        public bool Expires { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public string? Error { get; set; }
    }

    public interface ILicenseKeyService
    {
        string Generate(string type, int seats, bool expires, DateTime? expiresAt);
        LicenseValidationResult Decrypt(string licenseKey);
    }

    public class LicenseKeyService : ILicenseKeyService
    {
        private readonly byte[] _masterKey;

        public LicenseKeyService(IConfiguration configuration)
        {
            var keyBase64 = configuration["License:MasterKey"];
            if (string.IsNullOrEmpty(keyBase64))
            {
                // Fallback for dev if not configured. Generate a simple 32-byte key: php -r "echo base64_encode(random_bytes(32));"
                // Using a hardcoded one so it matches across reboots for seed data if needed.
                _masterKey = Encoding.UTF8.GetBytes("12345678901234567890123456789012"); 
            }
            else
            {
                _masterKey = Convert.FromBase64String(keyBase64);
            }
        }

        public string Generate(string type, int seats, bool expires, DateTime? expiresAt)
        {
            var payload = new
            {
                t = type,
                s = seats,
                e = expires,
                ea = expires && expiresAt.HasValue ? new DateTimeOffset(expiresAt.Value).ToUnixTimeSeconds() : (long?)null
            };

            var json = JsonSerializer.Serialize(payload);
            var plaintext = Encoding.UTF8.GetBytes(json);

            using var aesGcm = new AesGcm(_masterKey, 16);
            var nonce = new byte[12]; // 12 bytes
            RandomNumberGenerator.Fill(nonce);

            var ciphertext = new byte[plaintext.Length];
            var tag = new byte[16]; // 16 bytes

            aesGcm.Encrypt(nonce, plaintext, ciphertext, tag);

            // Combine: nonce + tag + ciphertext
            var encryptedData = new byte[nonce.Length + tag.Length + ciphertext.Length];
            Buffer.BlockCopy(nonce, 0, encryptedData, 0, nonce.Length);
            Buffer.BlockCopy(tag, 0, encryptedData, nonce.Length, tag.Length);
            Buffer.BlockCopy(ciphertext, 0, encryptedData, nonce.Length + tag.Length, ciphertext.Length);

            // Hex encode
            var hex = Convert.ToHexString(encryptedData);

            // Add dashes every 4 chars
            var chunks = Enumerable.Range(0, hex.Length / 4)
                                   .Select(i => hex.Substring(i * 4, 4));
            var remainder = hex.Length % 4;
            if (remainder > 0)
            {
                chunks = chunks.Append(hex.Substring(hex.Length - remainder));
            }

            return string.Join("-", chunks);
        }

        public LicenseValidationResult Decrypt(string licenseKey)
        {
            try
            {
                var cleanKey = licenseKey.Replace("-", "");
                var encryptedData = Convert.FromHexString(cleanKey);

                var nonce = new byte[12];
                var tag = new byte[16];
                var ciphertext = new byte[encryptedData.Length - nonce.Length - tag.Length];

                Buffer.BlockCopy(encryptedData, 0, nonce, 0, nonce.Length);
                Buffer.BlockCopy(encryptedData, nonce.Length, tag, 0, tag.Length);
                Buffer.BlockCopy(encryptedData, nonce.Length + tag.Length, ciphertext, 0, ciphertext.Length);

                var plaintext = new byte[ciphertext.Length];

                using var aesGcm = new AesGcm(_masterKey, 16);
                aesGcm.Decrypt(nonce, ciphertext, tag, plaintext);

                var json = Encoding.UTF8.GetString(plaintext);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var type = root.GetProperty("t").GetString();
                var seats = root.GetProperty("s").GetInt32();
                var expires = root.GetProperty("e").GetBoolean();
                long? ea = root.GetProperty("ea").ValueKind == JsonValueKind.Null ? null : root.GetProperty("ea").GetInt64();

                DateTime? expiresAtDate = null;
                bool isExpired = false;

                if (expires && ea.HasValue)
                {
                    expiresAtDate = DateTimeOffset.FromUnixTimeSeconds(ea.Value).UtcDateTime;
                    isExpired = DateTime.UtcNow > expiresAtDate;
                }

                return new LicenseValidationResult
                {
                    Valid = !isExpired,
                    Type = type,
                    Seats = seats,
                    Expires = expires,
                    ExpiresAt = expiresAtDate,
                    Error = null
                };
            }
            catch (Exception)
            {
                return new LicenseValidationResult
                {
                    Valid = false,
                    Seats = 0,
                    Expires = true,
                    Error = "License Key is invalid, corrupted, or tampered."
                };
            }
        }
    }
}
