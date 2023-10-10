using NetworkMessage.CommandsResaults;
using NetworkMessage.Cryptography;
using NetworkMessage.Cryptography.KeyStore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Printing.IndexedProperties;
using System.Reflection;
using System.Text;

namespace NetworkMessage
{
    public abstract class TcpClientCryptoCommunicator : INetworkCommunicator
    {
        protected readonly TcpClient client;
        protected readonly IAsymmetricCryptographer cryptographer;
        protected readonly AsymmetricKeyStoreBase keyStore;

        /// <summary>
        /// Внешний открытый ключ
        /// </summary>
        protected byte[] externalPublicKey;
        /// <summary>
        /// Личный закрытый ключ
        /// </summary>
        protected byte[] ownPrivateKey;

        /// <param name="cryptographer">Класс, предоставляющий методы ассиметричного шифрования</param>
        /// <exception cref="ArgumentNullException"></exception>
        public TcpClientCryptoCommunicator(TcpClient client, IAsymmetricCryptographer cryptographer,
            AsymmetricKeyStoreBase keyStore)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            this.client = client;

            if (cryptographer == null) throw new ArgumentNullException(nameof(cryptographer));
            this.cryptographer = cryptographer;

            if (keyStore == null) throw new ArgumentNullException(nameof(keyStore));
            this.keyStore = keyStore;
            ownPrivateKey = keyStore.PrivateKey;
        }

        public virtual void Send(INetworkObject networkObject)
        {
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);
            if (externalPublicKey == default)
                throw new NullReferenceException($"External public key was null. Use {nameof(SetExternalPublicKey)}");

            try
            {
                NetworkStream stream = client.GetStream();
                byte[] sizeWithEncodedData = ToNetworkMessageFormat(networkObject);
                stream.Write(sizeWithEncodedData);
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
                byte[] data = publicKeyResult.PublicKey;
                byte[] size = BitConverter.GetBytes(data.Length);
                byte[] sizeWithEncodedData = new byte[size.Length + data.Length];
                Buffer.BlockCopy(size, 0, sizeWithEncodedData, 0, size.Length);
                Buffer.BlockCopy(data, 0, sizeWithEncodedData, size.Length, data.Length);
                stream.WriteAsync(sizeWithEncodedData, 0, sizeWithEncodedData.Length);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public virtual async Task SendAsync(INetworkObject networkObject, CancellationToken token = default)
        {
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);
            if (externalPublicKey == default)
                throw new NullReferenceException($"External public key was null. Use {nameof(SetExternalPublicKey)}");

            try
            {
                NetworkStream stream = client.GetStream();
                byte[] sizeWithEncData = ToNetworkMessageFormat(networkObject);
                await stream.WriteAsync(sizeWithEncData, token);
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
                byte[] data = new byte[stream.Socket.Available];
                int readedBytes = stream.Read(data);
                if (readedBytes == 0) return default;

                string json = BytesToJson(data);
                return JsonToNetworkObject(json);
            }
            catch { throw; }
        }

        public virtual async Task<INetworkObject> ReceiveAsync(CancellationToken token = default)
        {
            if (!client.Connected) return default;
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[stream.Socket.Available];
                int readedBytes = await stream.ReadAsync(data, token);
                if (readedBytes == 0) return default;

                string json = BytesToJson(data);
                return JsonToNetworkObject(json);
            }
            catch { throw; }
        }        

        public virtual PublicKeyResult ReceivePublicKey()
        {
            if (!client.Connected) return default;
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] data = new byte[stream.Socket.Available];
                int readedBytes = stream.Read(data);
                if (readedBytes == 0) return default;

                Memory<byte> bytes = new Memory<byte>(data);
                int size = BitConverter.ToInt32(bytes.Slice(0, sizeof(int)).Span);
                if (size <= 0) return default;                
                PublicKeyResult publicKeyResult = new PublicKeyResult(bytes.Slice(sizeof(int)).ToArray());
                return publicKeyResult;
            }
            catch { throw; }
        }

        private byte[] ToNetworkMessageFormat(INetworkObject networkObject)
        {
            byte[] data = networkObject.ToByteArray(); //конвертирум данные в массив байт
            byte[] size = BitConverter.GetBytes(data.Length); //получаем размер данных
            data = cryptographer.Encrypt(data, externalPublicKey); //шифруем данные
            byte[] sizeWithEncData = new byte[size.Length + data.Length];

            Buffer.BlockCopy(size, 0, sizeWithEncData, 0, size.Length);
            Buffer.BlockCopy(data, 0, sizeWithEncData, size.Length, data.Length);
            return sizeWithEncData;
        }

        private string BytesToJson(byte[] data)
        {
            Memory<byte> bytes = new Memory<byte>(data);
            if (bytes.IsEmpty) throw new ArgumentNullException(nameof(bytes));

            int size = BitConverter.ToInt32(bytes.Slice(0, sizeof(int)).Span);
            if (size <= 0) return default;
            bytes = cryptographer.Decrypt(bytes.Slice(sizeof(int)).ToArray(), ownPrivateKey = keyStore.PrivateKey);
            return Encoding.UTF8.GetString(bytes.Span);
        }

        private INetworkObject JsonToNetworkObject(string json)
        {
            JObject receivedJsonObject = JObject.Parse(json);
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
            if (externalPublicKey == null) throw new ArgumentNullException(nameof(externalPublicKey));
            this.externalPublicKey = externalPublicKey;
        }

        public abstract void Handshake();
    }
}
