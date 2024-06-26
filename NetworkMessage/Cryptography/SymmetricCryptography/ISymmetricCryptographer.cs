﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Cryptography.SymmetricCryptography
{
    /// <summary>
    /// Предоставляет методы для реализации симметричного шифрования
    /// </summary>
    public interface ISymmetricCryptographer 
    {
        public byte[] Key { get; }

        public byte[] IV { get; }

        byte[] Encrypt(byte[] data, byte[] key, byte[] IV);

        /// <summary>
        /// Зашифровать данные. Поддерживает ожидание
        /// </summary>
        /// <param name="data">Данные для зашифровки</param>
        /// <param name="key">Ключ, при помощи которого будут зашифрованы данные</param>
        /// <param name="IV">Соль, обеспечивающая дополнительную безопасность</param>
        /// <returns>Зашифрованнные данные</returns>
        Task<byte[]> EncryptAsync(byte[] data, byte[] key, byte[] IV, CancellationToken token = default);

        byte[] Decrypt(byte[] encryptedData, byte[] key, byte[] IV);

        /// <summary>
        /// Расшифровать данные. Поддерживает ожидание
        /// </summary>
        /// <param name="encryptedData">Зашифрованные данные</param>
        /// <param name="key">Ключ, при помощи которого будут расшифрованы данные</param>
        /// <param name="IV">Соль, использовавшиеся при шифровании</param>
        /// <returns>Расшифрованные данные</returns>
        Task<byte[]> DecryptAsync(byte[] encryptedData, byte[] key, byte[] IV, CancellationToken token = default);        

        public ICryptoTransform CreateEncryptor(byte[] key, byte[] IV);

        public ICryptoTransform CreateDecryptor(byte[] key, byte[] IV);
    }
}
