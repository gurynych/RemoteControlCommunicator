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
        /*public byte[] Encrypt(byte[] data, byte[] key, byte[] IV)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (Aes aes = Aes.Create())
            {
                ConfigureAes(aes, key, IV);
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(data);                    
                    cryptoStream.FlushFinalBlock();           
                    return StreamToArray(memoryStream, data);
                }
            }
        }

        public async Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[] IV, CancellationToken token = default)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (Aes aes = Aes.Create())
            {
                ConfigureAes(aes, key, IV);
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    await cryptoStream.WriteAsync(data, token);
                    await cryptoStream.FlushFinalBlockAsync(token);
                    return await StreamToArrayAsync(memoryStream, data, token);
                }
            }
        }

        public byte[] Decrypt(byte[] data, byte[] key, byte[] IV)
        {
            using (MemoryStream encryptedMemoryStream = new MemoryStream(data))
            using (Aes aes = Aes.Create())
            {
                ConfigureAes(aes, key, IV);
                using (CryptoStream cryptoStream = new CryptoStream(encryptedMemoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    return StreamToArray(cryptoStream, data);
                }
            }
        }

        public async Task<byte[]> DecryptAsync(byte[] data, byte[] key, byte[] IV, CancellationToken token = default)
        {
            using (MemoryStream encryptedMemoryStream = new MemoryStream(data))
            using (Aes aes = Aes.Create())
            {
                ConfigureAes(aes, key, IV);
                using (CryptoStream cryptoStream = new CryptoStream(encryptedMemoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    return await StreamToArrayAsync(cryptoStream, data, token);
                }
            }
        }

        private byte[] StreamToArray(Stream stream, byte[] data)
        {
            int bufferSize = 1024 * 1024 * 10; // 10 MB
            byte[] readData = new byte[(data.Length < bufferSize) ? data.Length : int.MaxValue];
            int commonRead = 0;
            for (int i = 0; i < data.Length; i += bufferSize)
            {
                int remainingBytes = Math.Min(bufferSize, data.Length - i);
                byte[] buffer = new byte[remainingBytes];
                int read = stream.Read(buffer, i, remainingBytes);
                Buffer.BlockCopy(buffer, 0, readData, commonRead, read);
                commonRead += read;
            }

            return readData.Take(commonRead).ToArray();
        }

        private async Task<byte[]> StreamToArrayAsync(Stream stream, byte[] data, CancellationToken token = default)
        {
            int bufferSize = 1024 * 1024 * 10; // 10 MB
            byte[] readData = new byte[(data.Length < bufferSize) ? data.Length : int.MaxValue];
            int commonRead = 0;
            for (int i = 0; i < data.Length; i += bufferSize)
            {
                int remainingBytes = Math.Min(bufferSize, data.Length - i);
                byte[] buffer = new byte[remainingBytes];
                int read = await stream.ReadAsync(buffer, i, remainingBytes, token);
                Buffer.BlockCopy(buffer, 0, readData, commonRead, read);
                commonRead += read;
            }

            return readData.Take(commonRead).ToArray();
        }*/

        private readonly Aes aes;

        public byte[] Key => aes.Key;

        public byte[] IV => aes.IV;

        public AESCryptographer()
        {
            aes = Aes.Create();
            aes.KeySize = 256;
            //aes.BlockSize = 8 * 1024 * 1024 * 4;
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

        private void ConfigureAes(Aes aes, byte[] key = null, byte[] IV = null)
        {
            aes.KeySize = 256;
            //aes.BlockSize = 128;
            if (key != null && IV != null)
            {
                aes.Key = key;
                aes.IV = IV;
            }
            /*aes.Padding = PaddingMode.Zeros;
            aes.Mode = CipherMode.CBC;*/
        }

        public byte[] Encrypt(byte[] data, byte[] key, byte[] IV)
        {
            using (Aes aes = Aes.Create())
            {
                ConfigureAes(aes, key, IV);
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    return PerformCryptography(data, encryptor);
            }
        }

        public async Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[] IV, CancellationToken token = default)
        {
            using (Aes aes = Aes.Create())
            {
                ConfigureAes(aes, key, IV);
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    return await PerformCryptographyAsync(data, encryptor, token);
            }
        }

        public byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] IV)
        {
            using (Aes aes = Aes.Create())
            {
                ConfigureAes(aes, key, IV);
                using (ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    return PerformCryptography(encryptedData, encryptor);
            }
        }

        public async Task<byte[]> DecryptAsync(byte[] encryptedData, byte[] key, byte[] IV, CancellationToken token = default)
        {
            using (Aes aes = Aes.Create())
            {
                ConfigureAes(aes, key, IV);
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

        public byte[] GenerateIV()
        {
            using Aes aes = Aes.Create();
            ConfigureAes(aes);
            aes.GenerateIV();
            return aes.IV;
        }

        public byte[] GenerateKey()
        {
            using Aes aes = Aes.Create();
            ConfigureAes(aes);
            aes.GenerateKey();
            return aes.Key;
        }
    }
}
