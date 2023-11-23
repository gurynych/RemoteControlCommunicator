using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Cryptography
{
    public interface ISymmetricCryptographer : ICryptographer
    {
        /// <summary>
        /// Зашифровать данные
        /// </summary>
        /// <param name="data">Данные для зашифровки</param>
        /// <param name="publicKey">Ключ, при помощи которого будут зашифрованы данные</param>
        /// <returns>Зашифрованнные данные</returns>
        //byte[] ICryptographer.Encrypt(byte[] data, byte[] publicKey);

        /// <summary>
        /// Расшифровать данные
        /// </summary>
        /// <param name="encryptedData">Зашифрованные данные</param>
        /// <param name="privateKey">Ключ, при помощи которого будут расшифрованы данные</param>
        /// <returns>Расшифрованные данные</returns>
        //byte[] Decrypt(byte[] encryptedData, byte[] privateKey);

        byte[] GenerateKey();
    }
}
