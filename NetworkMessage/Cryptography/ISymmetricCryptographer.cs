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
        Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[] IV, CancellationToken token = default);

        /// <summary>
        /// Расшифровать данные
        /// </summary>
        /// <param name="encryptedData">Зашифрованные данные</param>
        /// <param name="key">Ключ, при помощи которого будут расшифрованы данные</param>
        /// <param name="IV">Соль, использовавшиеся при шифровании</param>
        /// <returns>Расшифрованные данные</returns>
        Task<byte[]> DecryptAsync(byte[] encryptedData, byte[] key, byte[] IV, CancellationToken token = default);

        byte[] GenerateKey();

        byte[] GenerateIV();
    }
}
