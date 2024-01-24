using NetworkMessage.Cryptography.AsymmetricCryptography;
using NetworkMessage.Cryptography.SymmetricCryptography;
using Newtonsoft.Json;

namespace NetworkMessage
{
    public class NetworkMessage : INetworkMessage
    {
        /// <summary>
        /// 64 Мб
        /// </summary>
        [JsonIgnore]
        public const int PackageSize = 1024 * 1024 * 64;
        [JsonIgnore]
        private readonly INetworkObject networkObject = null;        

        public async Task<byte[]> ToByteArrayAsync(byte[] asymmetricPublicKey,
            IAsymmetricCryptographer asymmetricCryptographer,
            ISymmetricCryptographer symmetricCryptographer,
            CancellationToken token = default)
        {
            byte[] key = symmetricCryptographer.GenerateKey();
            byte[] IV = symmetricCryptographer.GenerateIV();
            byte[] rawData = networkObject.ToByteArray();

            byte[] data = (rawData.Length <= PackageSize) ?
                await EncryptShortData(rawData, symmetricCryptographer, key, IV, token) :
                await EncryptLongData(rawData, symmetricCryptographer, key, IV, token);

            key = asymmetricCryptographer.Encrypt(key, asymmetricPublicKey); // Шифрование симметричного ключа
            IV = asymmetricCryptographer.Encrypt(IV, asymmetricPublicKey); // Шифрование вектора инициализации

            int keyLength = key.Length;
            int IVLength = IV.Length;
            int dataLength = data.Length;

            byte[] keyLengthInBytes = BitConverter.GetBytes(keyLength);
            byte[] IVLengthInBytes = BitConverter.GetBytes(IVLength);
            
            int totalLength = keyLengthInBytes.Length + keyLength + IVLengthInBytes.Length + IVLength + dataLength; // Общая длина получаемого массива
            byte[] result = new byte[totalLength];
            Buffer.BlockCopy(keyLengthInBytes, 0, result, 0, keyLengthInBytes.Length); // Размер ключа
            Buffer.BlockCopy(key, 0, result, keyLengthInBytes.Length, keyLength); // Размер ключа и ключ
            Buffer.BlockCopy(IVLengthInBytes, 0, result, keyLengthInBytes.Length + keyLength, IVLengthInBytes.Length); // Размер ключа, ключ и размер вектора

            token.ThrowIfCancellationRequested();

            Buffer.BlockCopy(IV, 0, result, keyLengthInBytes.Length + keyLength + IVLengthInBytes.Length, IVLength); // Размер ключа, ключ, размер вектора и вектор
            Buffer.BlockCopy(data, 0, result,
                keyLengthInBytes.Length + keyLength + IVLengthInBytes.Length + IVLength, dataLength); // Размер ключа, ключ, размер вектора, вектор, данные

            token.ThrowIfCancellationRequested();

            return result;
        }

        /// <summary>
        /// Симметричное шифрование коротких данных
        /// </summary>
        /// <returns>Массив байт в формате: размер ключа->ключ->размер вектора->вектор->размер данных->данные</returns>
        private async Task<byte[]> EncryptShortData(byte[] rawData,
            ISymmetricCryptographer symmetricCryptographer,
            byte[] key, byte[] IV,            
            CancellationToken token = default)
        {
            byte[] encrData = await symmetricCryptographer.EncryptAsync(rawData, key, IV, token);
            byte[] encrDataLengthInBytes = BitConverter.GetBytes(encrData.Length);
            byte[] lengthWithEncrData = new byte[sizeof(int) + encrData.Length];
            Buffer.BlockCopy(encrDataLengthInBytes, 0, lengthWithEncrData, 0, sizeof(int));
            Buffer.BlockCopy(encrData, 0, lengthWithEncrData, sizeof(int), encrData.Length);

            return lengthWithEncrData;
        }

        /// <summary>
        /// Симметричное шифрование больших данных путем их разбития на пакеты
        /// </summary>        
        /// <returns>Массив байт в формате: размер ключа->ключ->размер вектора->вектор->данные (размер пакета->пакет->размер пакета->пакет->)</returns>
        private async Task<byte[]> EncryptLongData(byte[] rawData,
            ISymmetricCryptographer symmetricCryptographer,
            byte[] key, byte[] IV,
            CancellationToken token = default)
        {
            List<byte[]> packages = new List<byte[]>();
            int commonBytes = 0;
            for (int i = 0; i < rawData.Length; i += PackageSize)
            {
                int remainingBytes = Math.Min(PackageSize, rawData.Length - i); // Размер незашифрованного пакета
                byte[] package = new byte[remainingBytes]; // Незашифрованный пакет
                Buffer.BlockCopy(rawData, i, package, 0, remainingBytes); // Заполнение незашифрованного пакета

                byte[] encryptedPackage = await symmetricCryptographer.EncryptAsync(package, key, IV, token); // Шифрование пакета
                byte[] encryptedPackageLengthInBytes = BitConverter.GetBytes(encryptedPackage.Length); // Размер зашифрованного пакета
                byte[] sizeWithEncryptedPackage = new byte[sizeof(int) + encryptedPackage.Length]; // Размер зашифрованного пакета и сам пакет

                Buffer.BlockCopy(encryptedPackageLengthInBytes, 0, sizeWithEncryptedPackage, 0, sizeof(int)); // Копирование размера пакета
                Buffer.BlockCopy(encryptedPackage, 0, sizeWithEncryptedPackage, sizeof(int), encryptedPackage.Length); // Копирование пакета

                packages.Add(sizeWithEncryptedPackage);
                commonBytes += sizeWithEncryptedPackage.Length;
            }

            int offset = 0;
            byte[] result = new byte[commonBytes];
            foreach (byte[] package in packages)
            {
                Buffer.BlockCopy(package, 0, result, offset, package.Length);
                offset += package.Length;
            }

            return result;
        }

        public NetworkMessage(INetworkObject networkObject)
        {
            this.networkObject = networkObject ?? throw new ArgumentNullException(nameof(networkObject));
        }
    }
}
