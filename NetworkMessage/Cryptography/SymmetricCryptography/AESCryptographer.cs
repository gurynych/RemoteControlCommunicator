using Newtonsoft.Json.Linq;
using System.IO;
using System.IO.Pipes;
using System.Security.Cryptography;

namespace NetworkMessage.Cryptography.SymmetricCryptography
{
    /// <summary>
    /// Предоставляет реализацию синхронной криптографии, используя алгоритм AES
    /// </summary>
    public class AESCryptographer : ISymmetricCryptographer
    {
        private readonly Aes aes;

        public byte[] Key => aes.Key;

        public byte[] IV => aes.IV;

        public AESCryptographer()
        {
            aes = Aes.Create();
            aes.GenerateKey();
            aes.GenerateIV();
        }

        public ICryptoTransform CreateEncryptor(byte[] key, byte[] IV)
        {
            aes.Key = key;
            aes.IV = IV;
            return aes.CreateEncryptor(key, IV);
        }

        public ICryptoTransform CreateDecryptor(byte[] key, byte[] IV)
        {
            aes.Key = key;
            aes.IV = IV;
            return aes.CreateDecryptor(key, IV);
        }        

        public byte[] Encrypt(byte[] data, byte[] key, byte[] IV)
        {
            using (Aes aes = Aes.Create())
            {                
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    return PerformCryptography(data, encryptor);
            }
        }

        public async Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[] IV, CancellationToken token = default)
        {
            using (Aes aes = Aes.Create())
            {                
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    return await PerformCryptographyAsync(data, encryptor, token);
            }
        }

        public byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] IV)
        {
            using (Aes aes = Aes.Create())
            {                
                using (ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    return PerformCryptography(encryptedData, encryptor);
            }
        }

        public async Task<byte[]> DecryptAsync(byte[] encryptedData, byte[] key, byte[] IV, CancellationToken token = default)
        {
            using (Aes aes = Aes.Create())
            {
                using (ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    return await PerformCryptographyAsync(encryptedData, encryptor, token);
            }
        }

        private byte[] PerformCryptography(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        private async Task<byte[]> PerformCryptographyAsync(byte[] data, ICryptoTransform cryptoTransform, CancellationToken token = default)
        {
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                await cryptoStream.WriteAsync(data, 0, data.Length, token);
                await cryptoStream.FlushFinalBlockAsync(token);
                return ms.ToArray();
            }
        }
    }
}
