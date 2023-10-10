namespace NetworkMessage.Cryptography
{
    public interface IAsymmetricCryptographer
    {
        /// <summary>
        /// Зашифровать данные
        /// </summary>
        /// <param name="data">Данные для зашифровки</param>
        /// <param name="publicKey">Ключ, при помощи которого будут зашифрованы данные</param>
        /// <returns>Зашифрованнные данные</returns>
        byte[] Encrypt(byte[] data, byte[] publicKey);

        /// <summary>
        /// Расшифровать данные
        /// </summary>
        /// <param name="encryptedData">Зашифрованные данные</param>
        /// <param name="privateKey">Ключ, при помощи которого будут расшифрованы данные</param>
        /// <returns>Расшифрованные данные</returns>
        byte[] Decrypt(byte[] encryptedData, byte[] privateKey);

        /// <summary>
        /// Сгенерировать закрытый ключ
        /// </summary>
        /// <param name="length">Длина генерируемого ключа</param>
        /// <returns></returns>
        byte[] GeneratePrivateKey();

        /// <summary>
        /// Сгенерировать открытый ключ
        /// </summary>
        /// <param name="length">Длина генерируемого ключа</param>
        /// <returns></returns>
        byte[] GeneratePublicKey(byte[] privateKey);        
    }
}
