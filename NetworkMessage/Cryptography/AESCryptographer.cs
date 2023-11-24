using System.Security.Cryptography;

namespace NetworkMessage.Cryptography
{
    /// <summary>
    /// Предоставляет реализацию синхронной криптографии, используя алгоритм AES
    /// </summary>
    public class AESCryptographer : ISymmetricCryptographer
    {
        public async Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[] IV, CancellationToken token = default)
        {
            try
            {
                string strData = System.Text.Encoding.UTF8.GetString(data);
                using (Aes aes = Aes.Create())
                {
                    ICryptoTransform encryptor = aes.CreateEncryptor(key, IV);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using StreamWriter writer = new StreamWriter(cs);
                            {
                                await writer.WriteAsync(strData.ToCharArray(), token);
                            }

                        }

                        return ms.ToArray();
                    }
                }
            }
            catch { throw; }
        }

        public async Task<byte[]> DecryptAsync(byte[] encryptedData, byte[] key, byte[] IV, CancellationToken token = default)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {
                    ICryptoTransform encryptor = aes.CreateDecryptor(key, IV);
                    using (MemoryStream ms = new MemoryStream(encryptedData))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader reader = new StreamReader(cs))
                            {
                                var json = await reader.ReadToEndAsync();
                                return System.Text.Encoding.UTF8.GetBytes(json);
                            }
                        }
                    }
                }
            }
            catch { throw; }
        }        

        public byte[] GenerateIV()
        {
            using Aes aes = Aes.Create();
            aes.GenerateIV();
            return aes.IV;
        }

        public byte[] GenerateKey()
        {
            using Aes aes = Aes.Create();
            aes.GenerateKey();
            return aes.Key;
        }
    }
}
