using NetworkMessage.Cryptography;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage
{
    public interface INetworkMessage
    {
        byte[] EncryptedSymmetricKey { get; set; }

        byte[] EncryptedIV { get; set; }

        byte[] EncryptedNetworkObject { get; set; }

        /// <summary>
        /// Шифрование сообщения
        /// </summary>       
        /// <param name="asymmetricPublicKey">Открытый ключ, необходимый для асимметричного шифрования</param>
        /// <param name="asymmetricCryptographer">
        /// Предоставляет интерфейс для асимметричного шифрования данных. По умолчанию используется RSA алгоритм
        /// </param>
        /// <param name="symmetricCryptographer">
        /// Предоставляет интерфейс для симметричного шифрования данных. По умолчанию используется AES алгоритм
        /// </param>
        Task EncryptMessage(byte[] asymmetricPublicKey,
            IAsymmetricCryptographer asymmetricCryptographer = null,
            ISymmetricCryptographer symmetricCryptographer = null,
            CancellationToken token = default);

        string ToString();

        byte[] ToByteArray();
    }
}
