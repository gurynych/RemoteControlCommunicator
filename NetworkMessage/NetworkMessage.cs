using NetworkMessage.Cryptography;

namespace NetworkMessage
{
    public class NetworkMessage : INetworkMessage
    {              
        private readonly INetworkObject networkObject;

        public byte[] EncryptedSymmetricKey { get; set; }

        public byte[] EncryptedIV { get; set; }

        public byte[] EncryptedNetworkObject { get; set; }

        public NetworkMessage()
        {            
        }

        public NetworkMessage(INetworkObject networkObject)
        {
            this.networkObject = networkObject ?? throw new ArgumentNullException(nameof(networkObject));
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

        public async Task EncryptMessage(byte[] asymmetricPublicKey, 
            IAsymmetricCryptographer asymmetricCryptographer = null,
            ISymmetricCryptographer symmetricCryptographer = null,
            CancellationToken token = default)
        {
            IAsymmetricCryptographer asymCr = asymmetricCryptographer ?? new RSACryptographer();
            ISymmetricCryptographer symCr = symmetricCryptographer ?? new AESCryptographer();

            byte[] key = symCr.GenerateKey();
            byte[] IV = symCr.GenerateIV();

            EncryptedNetworkObject = await symCr.EncryptAsync(networkObject.ToByteArray(), key, IV, token);
            EncryptedSymmetricKey = asymCr.Encrypt(key, asymmetricPublicKey);
            EncryptedIV = asymCr.Encrypt(IV, asymmetricPublicKey);
        }
    }
}
