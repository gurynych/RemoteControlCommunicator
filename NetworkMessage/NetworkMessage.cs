using NetworkMessage.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace NetworkMessage
{
    public class NetworkMessage : INetworkMessage
    {              
        public byte[] EncryptedSymmetricKey { get; set; }

        public byte[] EncryptedIV { get; set; }

        public byte[] EncryptedNetworkObject { get; set; }

        public NetworkMessage()
        {            
        }

        public NetworkMessage(INetworkObject networkObject,
            IAsymmetricCryptographer asymmetricCryptographer, 
            ISymmetricCryptographer symmetricCryptographer,
            byte[] publicKey)
        {
            byte[] key = symmetricCryptographer.GenerateKey();
            byte[] IV = symmetricCryptographer.GenerateIV();

            EncryptedNetworkObject = symmetricCryptographer.Encrypt(networkObject.ToByteArray(), key, IV);
            EncryptedSymmetricKey = asymmetricCryptographer.Encrypt(key, publicKey);
            EncryptedIV = asymmetricCryptographer.Encrypt(IV, publicKey);
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public byte[] ToByteArray()
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(ToString());
            byte[] dataLength = BitConverter.GetBytes(data.Length);
            byte[] result = new byte[dataLength.Length + data.Length];
            Buffer.BlockCopy(dataLength, 0, result, 0, dataLength.Length);
            Buffer.BlockCopy(data, 0, result, dataLength.Length, data.Length);
            return result;
        }

    }
}
