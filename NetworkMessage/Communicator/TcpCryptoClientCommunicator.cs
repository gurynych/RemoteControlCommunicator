using NetworkMessage.Cryptography.AsymmetricCryptography;
using NetworkMessage.Cryptography.KeyStore;
using NetworkMessage.Cryptography.SymmetricCryptography;
using NetworkMessage.Exceptions;
using NetworkMessage.Intents;
using NetworkMessage.DTO;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using NetworkMessage.CommandsResults.ConcreteCommandResults;
using NetworkMessage.Intents.ConcreteIntents;

namespace NetworkMessage.Communicator
{
    public abstract class TcpCryptoClientCommunicator : INetworkCommunicator
    {
        private const int BufferSize = 1024 * 1024;
        private bool isConnected;
        protected TcpClient client;
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

        public bool IsConnected
        {
            get => isConnected && client.Connected;
            set => isConnected = value;
        }

        /// <exception cref="ArgumentNullException"></exception>
        public TcpCryptoClientCommunicator(TcpClient client, IAsymmetricCryptographer asymmetricCryptographer,
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

        public async Task<bool> ReconnectWithHandshakeAsync(string serverIP, int serverPort, IProgress<long> progress = null, CancellationToken token = default)
        {
            client.Close();
            client = new TcpClient();
            IsConnected = await ConnectAsync(serverIP, serverPort, token).ConfigureAwait(false);
            if (IsConnected)
            {
                try
                {
                    await HandshakeAsync(progress, token).ConfigureAwait(false);
                    IsConnected = true;
                }
                catch
                {
                    IsConnected = false;
                }
            }

            return IsConnected;
        }

        public async Task<bool> ConnectAsync(string serverIP, int serverPort, CancellationToken token = default)
        {
            try
            {
                await client.ConnectAsync(serverIP, serverPort, token).ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SendObjectAsync(INetworkObject message, IProgress<long> progress = null, CancellationToken token = default)
        {
            if (externalPublicKey == null || externalPublicKey.Length == 0)
                throw new NullReferenceException($"External public key was null. Use {nameof(SetExternalPublicKey)}");
            
            await SendStreamAsync(message.ToStream(), progress, token).ConfigureAwait(false);
        }

        private async Task SendStreamAsync(Stream streamToSend, IProgress<long> progress = null, CancellationToken token = default) //(byte[] rawBytes, IProgress<long> progress = null, CancellationToken token = default)
        {
            if (!client.Connected) throw new DeviceNotConnectedException();

            streamToSend.Position = 0;
            long streamLength = streamToSend.Length;
            NetworkStream networkStream = client.GetStream();
            BufferedStream bufferedStream = new BufferedStream(networkStream);
            byte[] cryptoKey = asymmetricCryptographer.Encrypt(symmetricCryptographer.Key, externalPublicKey);
            byte[] cryptoIV = asymmetricCryptographer.Encrypt(symmetricCryptographer.IV, externalPublicKey);
            byte[] sizeKey = BitConverter.GetBytes(cryptoKey.Length);
            byte[] sizeIV = BitConverter.GetBytes(cryptoIV.Length);
            
            await networkStream.WriteAsync(sizeKey, token).ConfigureAwait(false);
            await networkStream.WriteAsync(cryptoKey, token).ConfigureAwait(false);
            await networkStream.WriteAsync(sizeIV, token).ConfigureAwait(false);
            await networkStream.WriteAsync(cryptoIV, token).ConfigureAwait(false);
            await networkStream.WriteAsync(BitConverter.GetBytes(streamLength), token).ConfigureAwait(false);

            using (CryptoStream cryptoStream =
                new CryptoStream(networkStream,
                    symmetricCryptographer.CreateEncryptor(symmetricCryptographer.Key, symmetricCryptographer.IV),
                    CryptoStreamMode.Write,
                    true))
            {
                long commonSend = 0;
                long sizeToSend;
                byte[] buffer = new byte[BufferSize];
                do
                {
                    sizeToSend = Math.Min(BufferSize, streamLength - commonSend);
                    await streamToSend.ReadExactlyAsync(buffer.AsMemory(0, (int)sizeToSend), token).ConfigureAwait(false);
                    await cryptoStream.WriteAsync(buffer.AsMemory(0, (int)sizeToSend), token).ConfigureAwait(false);
                    commonSend += sizeToSend;
                    progress?.Report(commonSend);
                } while (sizeToSend > 0 && commonSend < streamLength);
            }

            await bufferedStream.FlushAsync(token);
        }

        public async Task<TNetworkObject> ReceiveAsync<TNetworkObject>(IProgress<long> progress = null, CancellationToken token = default)
            where TNetworkObject : INetworkObject
        {            
            byte[] data;
            using (MemoryStream stream = new MemoryStream())
            {
                await ReceiveStreamAsync(stream, progress, token).ConfigureAwait(false);
                data = stream.ToArray();
            }

            string networkObjectJson = Encoding.UTF8.GetString(data);
            return JsonConvert.DeserializeObject<TNetworkObject>(networkObjectJson);
        }        

        public async Task ReceiveStreamAsync(Stream streamToWrite, IProgress<long> progress = null, CancellationToken token = default)
        {            
            if (!client.Connected) throw new DeviceNotConnectedException();

            NetworkStream networkStream = client.GetStream();
            ArgumentNullException.ThrowIfNull(nameof(networkStream));
            ConfigurationData confData = await ReceiveConfigurationData(networkStream, token).ConfigureAwait(false);
            //Для уведомления networkStream и cryptoStream об окончании потока
            using MemoryStream memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream =
                new CryptoStream(streamToWrite, symmetricCryptographer.CreateDecryptor(confData.SymKey, confData.SymIV), CryptoStreamMode.Write, true))
            {
                long commonRead = 0;
                byte[] buffer = new byte[BufferSize];
                long sizeToRead;
                do
                {
                    sizeToRead = Math.Min(buffer.Length, confData.MessageSize - commonRead);
                    await networkStream.ReadExactlyAsync(buffer, 0, (int)sizeToRead, token).ConfigureAwait(false);
                    await memoryStream.WriteAsync(buffer.AsMemory(0, (int)sizeToRead), token).ConfigureAwait(false);
                    await cryptoStream.WriteAsync(memoryStream.ToArray().AsMemory(0, (int)sizeToRead), token).ConfigureAwait(false);                    
                    memoryStream.Position = 0;
                    commonRead += sizeToRead;
                    progress?.Report(commonRead);
                } while (sizeToRead > 0 && commonRead < confData.MessageSize);
            }
        }

        private async Task<ConfigurationData> ReceiveConfigurationData(NetworkStream networkStream, CancellationToken token = default)
        {            
            byte[] symKey = await GetDecryptedData(networkStream, token).ConfigureAwait(false);
            byte[] symIV = await GetDecryptedData(networkStream, token).ConfigureAwait(false);

            byte[] longSize = new byte[sizeof(long)];
            _ = await networkStream.ReadAsync(longSize, token).ConfigureAwait(false);
            long messageSize = BitConverter.ToInt64(longSize);
            messageSize += 16 - (messageSize % 16);

            return new ConfigurationData(symKey, symIV, messageSize);
        }

        private async Task<byte[]> GetDecryptedData(NetworkStream networkStream, CancellationToken token = default)
        {
            byte[] dataSizeInBytes = new byte[sizeof(int)];
            _ = await networkStream.ReadAsync(dataSizeInBytes, token).ConfigureAwait(false);
            int dataSize = BitConverter.ToInt32(dataSizeInBytes);
            byte[] data = new byte[dataSize];
            _ = await networkStream.ReadAsync(data, token).ConfigureAwait(false);
            return asymmetricCryptographer.Decrypt(data, ownPrivateKey);
        }

        public async Task<BaseIntent> ReceiveAsync(IProgress<long> progress = null, CancellationToken token = default)
        {            
            using MemoryStream stream = new MemoryStream();
            await ReceiveStreamAsync(stream, progress, token).ConfigureAwait(false);
            return GetIntentFromBytes(stream.ToArray(), token);
        }

        /// <summary>
        /// Установить внешний окрытый ключ
        /// </summary>
        /// <param name="externalPublicKey">Внешний ключ</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetExternalPublicKey(byte[] externalPublicKey)
        {
            this.externalPublicKey = externalPublicKey ?? throw new ArgumentNullException(nameof(externalPublicKey));
        }

        public abstract Task<bool> HandshakeAsync(IProgress<long> progress = null, CancellationToken token = default);
        
        /// <summary>
        /// Рукопожатие
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="token"></param>
        /// <returns>Guid подключенного устройства</returns>
        /// <exception cref="NullReferenceException"></exception>
        public virtual async Task<GuidResult> HandshakeAsync1(IProgress<long> progress = null, CancellationToken token = default)
        {
            byte[] publicKey;
            using (MemoryStream ms = new MemoryStream())
            {
                await ReceiveStreamAsync(ms, progress, token).ConfigureAwait(false);
                publicKey = ms.ToArray();
            }

            if (publicKey == default || publicKey.Length == 0) throw new NullReferenceException(nameof(publicKey));

            SetExternalPublicKey(publicKey);
            BaseIntent guidIntent = new GuidIntent();            
            await SendObjectAsync(guidIntent, progress, token).ConfigureAwait(false);
            return await ReceiveAsync<GuidResult>(progress, token).ConfigureAwait(false);
        }

        private BaseIntent GetIntentFromBytes(byte[] data, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            string networkObjectJson = Encoding.UTF8.GetString(data);
            Intent intent = JsonConvert.DeserializeObject<Intent>(networkObjectJson);
            if (intent == null) return default;
            
            Type intentType = IntentConverter.GetType(intent.IntentType);
            if (intentType == null) return default;

            return (BaseIntent)JsonConvert.DeserializeObject(networkObjectJson, intentType);
        }        

        public void Dispose()
        {
            client.Close();
        }
    }
}
