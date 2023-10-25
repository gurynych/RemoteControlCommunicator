using NetworkMessage.CommandsResaults;
using NetworkMessage.Cryptography;
using NetworkMessage.Cryptography.KeyStore;
using Newtonsoft.Json.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
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
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[512];                
                int bytesAvailable = stream.Socket.Available;
                do
                {                    
                    stream.Read(buffer, data.Count, buffer.Length);
                    data.AddRange(buffer);
                } while (stream.Socket.Available > 0);

                string json = BytesToJson(data.ToArray());
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
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[512];
                //byte[] buffer = new byte[1024];
                //int readedBytes = await stream.ReadAsync(buffer, token);
                //buffer = buffer.Take(readedBytes).ToArray();
                int bytesAvailable = stream.Socket.Available;
                do
                {                    
                    //int count = Math.Min(bytesAvailable, buffer.Length);
                    //bytesAvailable -= await stream.ReadAsync(buffer, data.Count, count, token);
                    await stream.ReadAsync(buffer, data.Count, buffer.Length, token);
                    data.AddRange(buffer);
                    //await stream.ReadAsync(buffer, token);
                } while (stream.Socket.Available > 0);
                //if (readedBytes == 0) return default;

                //string json = BytesToJson(buffer);
                string json = BytesToJson(data.ToArray());
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
                List<byte> data = new List<byte>();
                byte[] buffer = new byte[512];                
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
                byte[] buffer = new byte[512];
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

        private byte[] PublicKeyToNetworkMessageFormat(byte[] publicKey)
        {
            byte[] size = BitConverter.GetBytes(publicKey.Length);
            byte[] sizeWithPublickey = new byte[size.Length + publicKey.Length];
            Buffer.BlockCopy(size, 0, sizeWithPublickey, 0, size.Length);
            Buffer.BlockCopy(publicKey, 0, sizeWithPublickey, size.Length, publicKey.Length);
            return sizeWithPublickey;
        }

        private byte[] ToNetworkMessageFormat(INetworkObject networkObject)
        {
            byte[] data = networkObject.ToByteArray(); //конвертирум данные в массив байт
            data = cryptographer.Encrypt(data, externalPublicKey); //шифруем данные
            byte[] size = BitConverter.GetBytes(data.Length); //получаем размер данных
            byte[] sizeWithEncData = new byte[size.Length + data.Length];

            Buffer.BlockCopy(size, 0, sizeWithEncData, 0, size.Length);
            Buffer.BlockCopy(data, 0, sizeWithEncData, size.Length, data.Length);
            return sizeWithEncData;
        }

        private string BytesToJson(byte[] data)
        {            
            if (data == default) throw new ArgumentNullException(nameof(data));

            int size = BitConverter.ToInt32(data.Take(sizeof(int)).ToArray());           
            if (size <= 0) return default;
            data = data.Skip(sizeof(int)).Take(size).ToArray();
            data = cryptographer.Decrypt(data, ownPrivateKey = keyStore.PrivateKey);
            return Encoding.UTF8.GetString(data);
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
    }
}
