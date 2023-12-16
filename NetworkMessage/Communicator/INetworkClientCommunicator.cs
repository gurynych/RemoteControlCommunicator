using NetworkMessage.CommandsResults;
using NetworkMessage.Intents;

namespace NetworkMessage.Communicator
{
    public interface INetworkClientCommunicator : IDisposable
    {
        bool IsConnected { get; }

        Task<bool> ConnectAsync(string serverIP, int serverPort, CancellationToken token = default);

        Task<bool> ReconnectWithHandshakeAsync(string serverIP, int serverPort, IProgress<int> progress = null, CancellationToken token = default);

        Task SendMessageAsync(INetworkMessage message, IProgress<int> progress = null, CancellationToken token = default);

        Task<byte[]> ReceiveBytesAsync(IProgress<int> progress = null, CancellationToken token = default);

        Task<BaseIntent> ReceiveIntentAsync(IProgress<int> progress = null, CancellationToken token = default);

        Task<TNetworkObject> ReceiveNetworkObjectAsync<TNetworkObject>(IProgress<int> progress = null, CancellationToken token = default)
            where TNetworkObject : INetworkObject;

        Task<bool> HandshakeAsync(IProgress<int> progress = null, CancellationToken token = default);
    }
}