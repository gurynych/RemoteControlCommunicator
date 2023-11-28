using NetworkMessage.CommandsResults;
using NetworkMessage.Cryptography.AsymmetricCryptography;
using NetworkMessage.Cryptography.KeyStore;
using NetworkMessage.Cryptography.SymmetricCryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace NetworkMessage
{
    public abstract class TcpCommunicator : INetworkCommunicator
    {
        private const int bytesForRead = 512;
        protected readonly TcpClient client;
        protected readonly IAsymmetricCryptographer asymmetricCryptographer;
        protected readonly ISymmetricCryptographer symmetricCryptographer;
        protected readonly AsymmetricKeyStoreBase keyStore;

        /// <summary>
        /// Внешний открытый ключ
        /// </summary>
        protected byte[] externalPublicKey;
        /// <summary>
        /// Личный закрытый ключ
        /// </summary>
        protected byte[] ownPrivateKey;

        /// <exception cref="ArgumentNullException"></exception>
        public TcpCommunicator(TcpClient client, IAsymmetricCryptographer asymmetricCryptographer,
            ISymmetricCryptographer symmetricCryptographer,
            AsymmetricKeyStoreBase keyStore)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.asymmetricCryptographer = asymmetricCryptographer ?? throw new ArgumentNullException(nameof(asymmetricCryptographer));
            this.symmetricCryptographer = symmetricCryptographer ?? throw new ArgumentNullException(nameof(symmetricCryptographer));
            this.symmetricCryptographer = symmetricCryptographer ?? throw new ArgumentNullException(nameof(symmetricCryptographer));
            this.keyStore = keyStore ?? throw new ArgumentNullException(nameof(keyStore));
            ownPrivateKey = keyStore.PrivateKey;
        }

        public virtual void Send(INetworkMessage networkMessage)
        {
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);
            if (externalPublicKey == default)
                throw new NullReferenceException($"External public key was null. Use {nameof(SetExternalPublicKey)}");

            try
            {
                networkMessage.EncryptMessageAsync(externalPublicKey).GetAwaiter().GetResult();
                NetworkStream stream = client.GetStream();                
                stream.Write(networkMessage.ToByteArrayRequiredFormat());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual void SendPublicKey(PublicKeyResult publicKeyResult)
        {
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);

            try
            {
                NetworkStream stream = client.GetStream();
                byte[] publicKey = publicKeyResult.PublicKey;
                byte[] sizeWithPublickey = PublicKeyToNetworkMessageFormat(publicKey);                
                stream.Write(sizeWithPublickey);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task SendPublicKeyAsync(PublicKeyResult publicKeyResult, CancellationToken token = default)
        {
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);

            try
            {
                NetworkStream stream = client.GetStream();
                byte[] publicKey = publicKeyResult.PublicKey;
                byte[] sizeWithPublickey = PublicKeyToNetworkMessageFormat(publicKey);
                await stream.WriteAsync(sizeWithPublickey, token);
            }
            catch (Exception)
            {
                throw;
            }
        }        

        public virtual async Task SendAsync(INetworkMessage networkMessage, CancellationToken token = default)
        {
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);
            if (externalPublicKey == default)
                throw new NullReferenceException($"External public key was null. Use {nameof(SetExternalPublicKey)}");

            try
            {
                await networkMessage.EncryptMessageAsync(externalPublicKey);
                NetworkStream stream = client.GetStream();
                await stream.WriteAsync(networkMessage.ToByteArrayRequiredFormat(), token);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual INetworkObject Receive()
        {
            if (!client.Connected) return default;
            try
            {
                NetworkStream stream = client.GetStream();
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[bytesForRead];                
                do
                {
                    stream.Read(buffer);
                    //stream.Read(buffer, data.Count, buffer.Length);
                    data.AddRange(buffer);
                } while (stream.Socket.Available > 0);

                INetworkMessage networkMessage = ReceivedBytesToNetworkMessage(data.ToArray());
                if (networkMessage == null) return default;

                return GetObjectFromMessage(networkMessage).Result;
            }
            catch { throw; }
        }

        public virtual async Task<INetworkObject> ReceiveAsync(CancellationToken token = default)
        {
            if (!client.Connected) return default;
            try
            {
                NetworkStream stream = client.GetStream();
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[bytesForRead];
                int bytesAvailable = stream.Socket.Available;
                do
                {
                    await stream.ReadAsync(buffer, token);
                    //await stream.ReadAsync(buffer, data.Count, buffer.Length, token);
                    data.AddRange(buffer);                   
                } while (stream.Socket.Available > 0);

                INetworkMessage networkMessage = ReceivedBytesToNetworkMessage(data.ToArray());
                if (networkMessage == null) return default;

                return await GetObjectFromMessage(networkMessage, token);

            }
            catch { throw; }
        }        

        public virtual PublicKeyResult ReceivePublicKey()
        {
            if (!client.Connected) return default;
            try
            {
                NetworkStream stream = client.GetStream();
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[bytesForRead];                
                int bytesAvailable = stream.Socket.Available;
                do
                {
                    stream.Read(buffer, data.Count, buffer.Length);
                    data.AddRange(buffer);                    
                } while (stream.Socket.Available > 0);

                int size = BitConverter.ToInt32(data.Take(sizeof(int)).ToArray());
                if (size <= 0) return default;
                byte[] publicKey = data.Skip(sizeof(int)).Take(size).ToArray();
                PublicKeyResult publicKeyResult = new PublicKeyResult(publicKey);
                return publicKeyResult;
            }
            catch { throw; }
        }        

        public virtual async Task<PublicKeyResult> ReceivePublicKeyAsync(CancellationToken token = default)
        {
            if (!client.Connected) return default;
            try
            {
                NetworkStream stream = client.GetStream();
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[bytesForRead];
                int bytesAvailable = stream.Socket.Available;
                do
                {
                    await stream.ReadAsync(buffer, data.Count, buffer.Length, token);
                    data.AddRange(buffer);
                } while (stream.Socket.Available > 0);

                int size = BitConverter.ToInt32(data.Take(sizeof(int)).ToArray());
                if (size <= 0) return default;
                byte[] publicKey = data.Skip(sizeof(int)).Take(size).ToArray();
                PublicKeyResult publicKeyResult = new PublicKeyResult(publicKey);
                return publicKeyResult;
            }
            catch { throw; }
        }

        public virtual void Stop()
        {
            client.Close();
        }

        /// <summary>
        /// Установить внешний окрытый ключ
        /// </summary>
        /// <param name="externalPublicKey">Внешний ключ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void SetExternalPublicKey(byte[] externalPublicKey)
        {
            this.externalPublicKey = externalPublicKey ?? throw new ArgumentNullException(nameof(externalPublicKey));
        }

        public abstract Task Handshake(CancellationToken token);

        protected virtual byte[] PublicKeyToNetworkMessageFormat(byte[] publicKey)
        {
            byte[] size = BitConverter.GetBytes(publicKey.Length);
            byte[] sizeWithPublickey = new byte[size.Length + publicKey.Length];
            Buffer.BlockCopy(size, 0, sizeWithPublickey, 0, size.Length);
            Buffer.BlockCopy(publicKey, 0, sizeWithPublickey, size.Length, publicKey.Length);
            return sizeWithPublickey;
        }

        protected virtual INetworkMessage ReceivedBytesToNetworkMessage(byte[] message)
        {
            const int lengthInfo = sizeof(int);
            if (message == default || message.Length < lengthInfo) throw new ArgumentNullException(nameof(message));

            int messageLength = BitConverter.ToInt32(message.Take(lengthInfo).ToArray());
            if (messageLength <= 0) return default;

            string json = Encoding.UTF8.GetString(message.Skip(lengthInfo).Take(messageLength).ToArray());
            INetworkMessage networkMessage = JsonConvert.DeserializeObject<NetworkMessage>(json);
            if (networkMessage == null) return default;

            return networkMessage;
        }

        protected virtual async Task<INetworkObject> GetObjectFromMessage(INetworkMessage networkMessage, CancellationToken token = default)
        {
            if (networkMessage == null) return default;

            byte[] symKey = asymmetricCryptographer.Decrypt(networkMessage.EncryptedSymmetricKey, ownPrivateKey);
            byte[] IV = asymmetricCryptographer.Decrypt(networkMessage.EncryptedIV, ownPrivateKey);
            byte[] networkObjectBytes = await symmetricCryptographer.DecryptAsync(networkMessage.EncryptedNetworkObject, symKey, IV, token);
            string networkObjectJson = Encoding.UTF8.GetString(networkObjectBytes);

            JObject receivedJsonObject = JObject.Parse(networkObjectJson);
            // Получаем значение поля "ObjectType" из JSON
            string objectType = receivedJsonObject[nameof(INetworkObject.NetworkObjectType)].ToString();
            // Получаем тип объекта по имени
            Type receivedObjectType = Type.GetType(objectType);
            // Если удалось получить тип объекта, десериализуем JSON в этот тип
            if (receivedObjectType != null)
            {
                INetworkObject receivedObject = (INetworkObject)receivedJsonObject.ToObject(receivedObjectType);
                return receivedObject;
            }

            return default;
        }        
    }
}
