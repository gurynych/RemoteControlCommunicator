using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Markup;

namespace NetworkMessage.Cryptography
{
    /// <summary>
    /// Предоставляет ассинхронной криптографии, используя алгоритм RSA.
    /// Длинна ключей 2048 бит
    /// </summary>
    public class RSACryptographer : IAsymmetricCryptographer
    {
        private const int KEY_SIZE = 2048;

        /// <summary>
        /// Дешифровать данные, используя закрытый ключ
        /// </summary>
        /// <param name="encryptedData">Зашифрованные данные</param>
        /// <param name="privateKey">Закрытый ключ для дешифровки зашифрованных данных</param>
        /// <returns></returns>
        /// <exception cref="CryptographicException"></exception>
        public byte[] Decrypt(byte[] encryptedData, byte[] privateKey)
        {
            try
            {
                using var rsa = new RSACryptoServiceProvider(KEY_SIZE);
                rsa.ImportCspBlob(privateKey);
                return rsa.Decrypt(encryptedData, true);
            }
            catch (CryptographicException cryptoEx)
            {
                throw cryptoEx;
            }
        }

        /// <summary>
        /// Зашифровать данные, используя открытый ключ
        /// </summary>
        /// <param name="data">Данные, которые необходимо зашифровать</param>
        /// <param name="publicKey">Открытый ключ, при помощи которого данные будут шифроваться</param>
        /// <returns>Данные после шифрования</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// /// <exception cref="CryptographicException"></exception>
        public byte[] Encrypt(byte[] data, byte[] publicKey)
        {

            if (data == null) throw new ArgumentNullException(nameof(data));
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));

            try
            {
                using var rsa = new RSACryptoServiceProvider(KEY_SIZE);
                rsa.ImportCspBlob(publicKey);
                return rsa.Encrypt(data, true);
            }
            catch (CryptographicException cryptoEx)
            {
                throw cryptoEx;
            }
        }

        /// <summary>
        /// Сгенерировать закрытый ключ
        /// </summary>
        /// <returns>Сгенерированный закрытый ключ, представляет собой массив байт, длинной 2048 бит</returns>
        public byte[] GeneratePrivateKey()
        {
            using var rsa = new RSACryptoServiceProvider(KEY_SIZE);
            return rsa.ExportCspBlob(true);
        }

        /// <summary>
        /// Сгенерировать открытый ключ
        /// </summary>
        /// <param name="privateKey">Закрытый ключ, на основе которого генерируется открытый ключ</param>
        /// <returns>Сгенерированный открытый ключ, представляет собой массив байт, длинной 2048 бит</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public byte[] GeneratePublicKey(byte[] privateKey)
        {            
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));

            using var rsa = new RSACryptoServiceProvider(KEY_SIZE);
            rsa.ImportCspBlob(privateKey);
            return rsa.ExportCspBlob(false);
        }
    }
}
