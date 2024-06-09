using System.Text;
using NetworkMessage.Cryptography.AsymmetricCryptography;

namespace NetworkMessage.Cryptography.KeyStore
{
    public abstract class AsymmetricKeyStoreBase
    {
        protected readonly IAsymmetricCryptographer cryptographer;
        public byte[] PrivateKey => SetPrivateKey();

        /// <param name="asymmetricCryptographer">Класс, предоставляющий методы для работы с асимметричными алгоритмами шифрования</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AsymmetricKeyStoreBase(IAsymmetricCryptographer asymmetricCryptographer)
        {
            ArgumentNullException.ThrowIfNull(asymmetricCryptographer, nameof(asymmetricCryptographer));
            cryptographer = asymmetricCryptographer;
        }

        protected abstract byte[] SetPrivateKey();

        public virtual byte[] GetPublicKey()
        {
            byte[] pubK = cryptographer.GeneratePublicKey(PrivateKey);
            return pubK;
        }

        protected virtual byte[] FromBytesToBase64Bytes(byte[] bytes)
        {
            string base64 = Convert.ToBase64String(bytes);
            return Encoding.UTF8.GetBytes(base64);
        }
    }
}