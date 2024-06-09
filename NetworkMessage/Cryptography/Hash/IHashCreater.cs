namespace NetworkMessage.Cryptography.Hash
{
    public interface IHashCreater
    {
        /// <summary>
        /// Сгенерировать соль
        /// </summary>
        /// <returns></returns>
        string GenerateSalt();

        /// <summary>
        /// Хеширование исходного текста с использованием соли
        /// </summary>
        /// <param name="data">Исходный текст</param>
        /// <param name="salt">Соль</param>
        /// <returns>Хеш исходной строки</returns>
        string Hash(string data, string salt);

        /// <summary>
        /// Хеширование исходного текста 
        /// </summary>
        /// <param name="data">Исходный текст</param>
        /// <returns>Хеш исходной строки</returns>
        string Hash(string data);
    }
}
