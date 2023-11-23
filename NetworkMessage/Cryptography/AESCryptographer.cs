using System;
using System.Security.Cryptography;

namespace NetworkMessage.Cryptography
{
    public class AESCryptographer : ISymmetricCryptographer
    {
        public byte[] Decrypt(byte[] encryptedData, byte[] key)
        {
            throw new NotImplementedException();
        }

        public byte[] Encrypt(byte[] data, byte[] key)
        {
            throw new NotImplementedException();
        }

        public byte[] GenerateKey()
        {
            Aes aes = Aes.Create();
            aes.Key = GenerateKey();
            return aes.Key;
        }
    }
}
