using Microsoft.VisualBasic;
using NetworkMessage.CommandsResults;
using NetworkMessage.Cryptography.AsymmetricCryptography;
using NetworkMessage.Cryptography.KeyStore;
using NetworkMessage.Cryptography.SymmetricCryptography;
using NetworkMessage.Intents;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices.ObjectiveC;
using System.Security.Cryptography;
using System.Text;

namespace NetworkMessage.Communicator
{
    public abstract class TcpCryptoClientCommunicator : INetworkCommunicator
    {
        private const int BufferSize = 256;
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

        public async Task<bool> ReconnectWithHandshakeAsync(string serverIP, int serverPort, IProgress<long> progress = null, CancellationToken token = default)
        {
            IsConnected = await ConnectAsync(serverIP, serverPort, token).ConfigureAwait(false);
            if (IsConnected)
            {
                IsConnected = await HandshakeAsync(progress, token);
            }

            return IsConnected;
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

        private async Task SendStreamAsync(Stream stream, IProgress<long> progress = null, CancellationToken token = default) //(byte[] rawBytes, IProgress<long> progress = null, CancellationToken token = default)
        {
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);

            stream.Position = 0;
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
            await networkStream.WriteAsync(BitConverter.GetBytes(stream.Length), token).ConfigureAwait(false);
            
            using (CryptoStream cryptoStream =
                new CryptoStream(stream,
                    symmetricCryptographer.CreateEncryptor(symmetricCryptographer.Key, symmetricCryptographer.IV),
                    CryptoStreamMode.Read,
                    true))
            {
                int read;
                long commonReaded = 0;
                byte[] buffer = new byte[BufferSize];
                while ((read = await cryptoStream.ReadAsync(buffer.AsMemory(0, BufferSize), token)) > 0)
                {
                    await networkStream.WriteAsync(buffer.AsMemory(0, read), token);
                    commonReaded += read;
                    progress?.Report(commonReaded);
                }
            }

            await bufferedStream.FlushAsync(token);
        }

        public async Task<TNetworkObject> ReceiveNetworkObjectAsync<TNetworkObject>(IProgress<long> progress = null, CancellationToken token = default)
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

        public async Task SendMessageAsync(INetworkObject message, IProgress<long> progress = null, CancellationToken token = default)
        {
            if (externalPublicKey == default || externalPublicKey.Length == 0)
                throw new NullReferenceException($"External public key was null. Use {nameof(SetExternalPublicKey)}");
            
            await SendStreamAsync(message.ToStream(), progress, token);
        }

        public async Task ReceiveStreamAsync(Stream streamToWrite, IProgress<long> progress = null, CancellationToken token = default)
        {
            //TODO: throw new NotConnectcedException();
            if (!client.Connected) throw new SocketException((int)SocketError.NotConnected);

            NetworkStream networkStream = client.GetStream();
            if (networkStream == null) throw new ArgumentNullException(nameof(networkStream));
                       
            byte[] symKeySizeBytes = new byte[sizeof(int)];
            await networkStream.ReadAsync(symKeySizeBytes, 0, symKeySizeBytes.Length, token).ConfigureAwait(false);
            int symKeySize = BitConverter.ToInt32(symKeySizeBytes);
            byte[] symKey = new byte[symKeySize];
            _ = await networkStream.ReadAsync(symKey, 0, symKeySize, token).ConfigureAwait(false);
            symKey = asymmetricCryptographer.Decrypt(symKey, ownPrivateKey);

            byte[] IVSizeBytes = new byte[sizeof(int)];
            _ = await networkStream.ReadAsync(IVSizeBytes, 0, IVSizeBytes.Length, token).ConfigureAwait(false);
            int IVSize = BitConverter.ToInt32(IVSizeBytes);
            byte[] IV = new byte[IVSize];
            _ = await networkStream.ReadAsync(IV, 0, IVSize, token).ConfigureAwait(false);
            IV = asymmetricCryptographer.Decrypt(IV, ownPrivateKey);

            byte[] longSize = new byte[sizeof(long)];
            _ = await networkStream.ReadAsync(longSize, token).ConfigureAwait(false);
            var messageSize = BitConverter.ToInt32(longSize);
            messageSize = (int)Math.Ceiling(messageSize / 16.0) * 16;

            using MemoryStream memoryStream = new MemoryStream();
            using (CryptoStream cryptoStream =
                new CryptoStream(streamToWrite, symmetricCryptographer.CreateDecryptor(symKey, IV), CryptoStreamMode.Write, true))
            {
                //await cryptoStream.CopyToAsync(memoryStream, token).ConfigureAwait(false);
                //await cryptoStream.ReadExactlyAsync(buffer, 0, size, token).ConfigureAwait(false);                
                int read = 0;
                int commonRead = 0;
                byte[] buffer = new byte[8 * 1024];
                int sizeToRead;
                do
                {
                    //TODO: own ReadExactlyAsync
                    sizeToRead = Math.Min(buffer.Length, messageSize - commonRead);     
                    await networkStream.ReadExactlyAsync(buffer, 0, sizeToRead, token).ConfigureAwait(false);
                    await memoryStream.WriteAsync(buffer.AsMemory(0, sizeToRead), token).ConfigureAwait(false);
                    await cryptoStream.WriteAsync(memoryStream.ToArray().AsMemory(0, sizeToRead), token).ConfigureAwait(false);
                    await cryptoStream.FlushAsync(token).ConfigureAwait(false);
                    memoryStream.Position = 0;                                        
                    commonRead += sizeToRead;
                    progress?.Report(commonRead);                    

                } while (sizeToRead > 0 && commonRead < messageSize);
            }
        }        

        public async Task<BaseIntent> ReceiveIntentAsync(IProgress<long> progress = null, CancellationToken token = default)
        {
            //byte[] rawBytes = await ReceiveStreamAsync(progress, token);
            using MemoryStream stream = new MemoryStream();
            await ReceiveStreamAsync(stream, progress, token);
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
                byte[] buffer = new byte[Math.Min(BufferSize, dataSize - commonRead)];
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

        public async Task<string> ReceiveFile(IProgress<long> progress = null, CancellationToken token = default)
        {
            var path = Path.GetTempFileName();
            using var stream = File.OpenWrite(path);            
            await ReceiveStreamAsync(stream, progress, token);
            return path;
        }
    }
}
