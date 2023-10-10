using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace NetworkMessage.Cryptography.KeyStore
{
    public abstract class AsymmetricKeyStoreBase
    {
        protected readonly IAsymmetricCryptographer cryptographer;
        public byte[] PrivateKey => SetPrivateKey();

        /// <param name="cryptographer">Класс, предоставляющий методы для работы с асимметричными алгоритмами шифрования</param>
        /// <exception cref="ArgumentNullException"></exception>
        public AsymmetricKeyStoreBase(IAsymmetricCryptographer cryptographer)
        {
            if (cryptographer == null) throw new ArgumentNullException(nameof(cryptographer));
            this.cryptographer = cryptographer;
        }

        protected abstract byte[] SetPrivateKey();

        public virtual byte[] GetPublicKey()
        {
            byte[] publicK = cryptographer.GeneratePublicKey(PrivateKey);
            return publicK;
            //return FromBytesToBase64Bytes(publicK);
        }

        protected virtual byte[] FromBytesToBase64Bytes(byte[] bytes)
        {
            string base64 = Convert.ToBase64String(bytes);
            return Encoding.UTF8.GetBytes(base64);
        }
    }
}