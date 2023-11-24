using System.Security.Cryptography;

namespace NetworkMessage.Cryptography
{
    /// <summary>
    /// Предоставляет реализацию синхронной криптографии, используя алгоритм AES
    /// </summary>
    public class AESCryptographer : ISymmetricCryptographer
    {
        private static readonly Aes aes;
        static AESCryptographer()
        {
            aes = Aes.Create();
            aes.KeySize = 128;
        }

        public byte[] Encrypt(byte[] data, byte[] key, byte[] IV)
        {
            try
            {
                string strData = System.Text.Encoding.UTF8.GetString(data);
                ICryptoTransform encryptor = aes.CreateEncryptor(key, IV);
                using MemoryStream ms = new MemoryStream();
                using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
                using StreamWriter writer = new StreamWriter(cs);
                writer.Write(strData);
                return ms.ToArray();
            }
            catch { throw; }
        }

        public byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] IV)
        {
            try
            {                
                ICryptoTransform encryptor = aes.CreateEncryptor(key, IV);
                using MemoryStream ms = new MemoryStream();
                using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Read);
                using StreamReader reader = new StreamReader(cs);
                return  System.Text.Encoding.UTF8.GetBytes(reader.ReadToEnd());
            }
            catch { throw; }
        }        

        public byte[] GenerateIV()
        {
            aes.GenerateIV();
            return aes.IV;
        }

        public byte[] GenerateKey()
        {
            aes.GenerateKey();
            return aes.Key;
        }
    }
}
