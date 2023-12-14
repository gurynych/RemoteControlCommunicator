using NetworkMessage.Cryptography.AsymmetricCryptography;
using NetworkMessage.Cryptography.KeyStore;
using NetworkMessage.Cryptography.SymmetricCryptography;
using NetworkMessage.Intents;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace NetworkMessage.Communicator
{
    public abstract class TcpCryptoClientCommunicator : INetworkClientCommunicator
    {
        private bool isConnected;
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

        public bool IsConnected
        {
            get
            {
                return isConnected && client.Connected;
            }
            set
            {
                isConnected = value;
            }
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

        public async Task<bool> ConnectAsync(string serverIP, int serverPort, CancellationToken token = default)
        {
            try
            {
                await client.ConnectAsync(serverIP, serverPort, token);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task SendRawBytesAsync(byte[] rawBytes, IProgress<int> progress = null, CancellationToken token = default)
        {
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);

            client.SendBufferSize = rawBytes.Length;
            NetworkStream networkStream = client.GetStream();
            BufferedStream bufferedStream = new BufferedStream(networkStream);
            using (MemoryStream memoryStream = new MemoryStream(rawBytes))
            {
                await memoryStream.CopyToAsync(networkStream, token);
                await bufferedStream.FlushAsync(token);
            }
        }

        public async Task<TNetworkObject> ReceiveNetworkObjectAsync<TNetworkObject>(IProgress<int> progress = null, CancellationToken token = default)
            where TNetworkObject : INetworkObject
        {
            byte[] rawData = await ReceiveRawBytesAsync(progress, token).ConfigureAwait(false);
            string networkObjectJson = Encoding.UTF8.GetString(rawData);
            return JsonConvert.DeserializeObject<TNetworkObject>(networkObjectJson);
        }

        public async Task SendMessageAsync(INetworkMessage message, IProgress<int> progress = null, CancellationToken token = default)
        {
            if (externalPublicKey == default || externalPublicKey.Length == 0)
                throw new NullReferenceException($"External public key was null. Use {nameof(SetExternalPublicKey)}");

            byte[] bytes = await message.ToByteArrayAsync(externalPublicKey, asymmetricCryptographer, symmetricCryptographer, token);
            await SendRawBytesAsync(bytes, progress, token);
        }

        public async Task<byte[]> ReceiveBytesAsync(IProgress<int> progress = null, CancellationToken token = default)
        {
            return await ReceiveRawBytesAsync(progress, token).ConfigureAwait(false);
        }

        private async Task<byte[]> ReceiveRawBytesAsync(IProgress<int> progress = null, CancellationToken token = default)
        {
            //TODO: throw new NotConnectcedException();
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);

            NetworkStream networkStream = client.GetStream();
            if (networkStream == null) throw new ArgumentNullException(nameof(networkStream));

            int commonRead = 0;
            byte[] ecnryptedSymmetricKey = await ReadPackageFromStreamAsync(networkStream, token).ConfigureAwait(false);
            if (ecnryptedSymmetricKey == default)
            {
                return default;
            }
            progress?.Report(commonRead += ecnryptedSymmetricKey.Length);

            byte[] ecnryptedIV = await ReadPackageFromStreamAsync(networkStream, token).ConfigureAwait(false);
            if (ecnryptedIV == default)
            {
                return default;
            }
            progress?.Report(commonRead += ecnryptedIV.Length);

            int commonData = 0;
            List<byte[]> packages = new List<byte[]>();
            while (true)
            {
                if (!networkStream.DataAvailable)
                {
                    break;
                }

                byte[] package = await ReadPackageFromStreamAsync(networkStream, token);
                if (package == default) break;
                progress?.Report(commonRead += package.Length);
                package = await DecryptRawBytesAsync(package, ecnryptedSymmetricKey, ecnryptedIV, token);
                packages.Add(package);
                commonData += package.Length;
            }

            int offset = 0;
            byte[] data = new byte[commonData];
            foreach (byte[] package in packages)
            {
                Buffer.BlockCopy(package, 0, data, offset, package.Length);
                offset += package.Length;
            }

            return data;
        }

        public async Task<BaseIntent> ReceiveIntentAsync(IProgress<int> progress = null, CancellationToken token = default)
        {
            byte[] rawBytes = await ReceiveRawBytesAsync(progress, token);
            return GetIntentFromBytes(rawBytes, token);
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

        public abstract Task<bool> HandshakeAsync(IProgress<int> progress = null, CancellationToken token = default);

        private BaseIntent GetIntentFromBytes(byte[] data, CancellationToken token = default)
        {
            token.ThrowIfCancellationRequested();
            string networkObjectJson = Encoding.UTF8.GetString(data);
            Intent intent = JsonConvert.DeserializeObject<Intent>(networkObjectJson);
            if (intent == null) return default;

            IntentConverter intentConverter = new IntentConverter();
            Type intentType = intentConverter.GetType(intent.IntentType);
            if (intentType == null) return default;

            return (BaseIntent)JsonConvert.DeserializeObject(networkObjectJson, intentType);
        }

        private async Task<byte[]> DecryptRawBytesAsync(byte[] encryptedData, byte[] ecnryptedSymmetricKey, byte[] ecnryptedIV, CancellationToken token = default)
        {
            if (encryptedData == default || encryptedData.Length == 0) throw new ArgumentNullException(nameof(encryptedData));
            if (ecnryptedSymmetricKey == default || ecnryptedSymmetricKey.Length == 0) throw new ArgumentNullException(nameof(ecnryptedSymmetricKey));
            if (ecnryptedIV == default || ecnryptedIV.Length == 0) throw new ArgumentNullException(nameof(ecnryptedIV));

            byte[] symmetricKey = asymmetricCryptographer.Decrypt(ecnryptedSymmetricKey, ownPrivateKey);
            byte[] IV = asymmetricCryptographer.Decrypt(ecnryptedIV, ownPrivateKey);

            return await symmetricCryptographer.DecryptAsync(encryptedData, symmetricKey, IV, token);
        }

        private async Task<byte[]> ReadPackageFromStreamAsync(NetworkStream networkStream, CancellationToken token = default)
        {
            if (networkStream == null) throw new ArgumentNullException(nameof(networkStream));
            
            byte[] sizeInBytes = new byte[sizeof(int)];
            _ = await networkStream.ReadAsync(sizeInBytes, token).ConfigureAwait(false);
            int dataSize = BitConverter.ToInt32(sizeInBytes);
            if (dataSize <= 0)
            {
                //return default;
                //TODO: throw new NoDataException();
                throw new SocketException((int)SocketError.NoData);
            }

            byte[] data = new byte[dataSize];
            int commonRead = 0;
            while (commonRead < dataSize)
            {
                byte[] buffer = new byte[dataSize - commonRead];
                int read = await networkStream.ReadAsync(buffer, 0, buffer.Length, token);
                Buffer.BlockCopy(buffer, 0, data, commonRead, read);
                commonRead += read;
            }
            
            return data;
        }

        private async Task<byte[]> ReadByByteAsync(NetworkStream networkStream, int dataSize, CancellationToken token = default)
        {
            return await Task.Run(() =>
            {
                List<byte> data = new List<byte>(dataSize);
                int commonRead = 0;
                int read;
                while (commonRead != dataSize)
                {
                    read = networkStream.ReadByte();
                    token.ThrowIfCancellationRequested();
                    if (read == -1)
                    {
                        throw new SocketException((int)SocketError.MessageSize);
                    }

                    commonRead++;
                    data.Add((byte)read);
                }

                return data.ToArray();
            });
        }

        public void Dispose()
        {
            client.Close();
        }
    }
}
