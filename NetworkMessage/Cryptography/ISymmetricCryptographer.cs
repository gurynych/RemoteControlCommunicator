using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Cryptography
{
    /// <summary>
    /// Предоставляет методы для реализации симметричного шифрования
    /// </summary>
    public interface ISymmetricCryptographer
    {
        /// <summary>
        /// Зашифровать данные
        /// </summary>
        /// <param name="data">Данные для зашифровки</param>
        /// <param name="key">Ключ, при помощи которого будут зашифрованы данные</param>
        /// <param name="IV">Соль, обеспечивающая дополнительную безопасность</param>
        /// <returns>Зашифрованнные данные</returns>
        byte[] Encrypt(byte[] data, byte[] key, byte[] IV);

        /// <summary>
        /// Расшифровать данные
        /// </summary>
        /// <param name="encryptedData">Зашифрованные данные</param>
        /// <param name="key">Ключ, при помощи которого будут расшифрованы данные</param>
        /// <param name="IV">Соль, использовавшиеся при шифровании</param>
        /// <returns>Расшифрованные данные</returns>
        byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] IV);

        byte[] GenerateKey();

        byte[] GenerateIV();
    }
}
