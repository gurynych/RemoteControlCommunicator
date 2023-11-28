using NetworkMessage.CommandsResults;

namespace NetworkMessage
{
    public interface INetworkCommunicator
    {
        public void Send(INetworkMessage networkMessage);

        public void SendPublicKey(PublicKeyResult publicKeyResult);

        public Task SendPublicKeyAsync(PublicKeyResult publicKeyResult, CancellationToken token = default);

        public INetworkObject Receive();

        public PublicKeyResult ReceivePublicKey();

        public Task<PublicKeyResult> ReceivePublicKeyAsync(CancellationToken token = default);

        Task SendAsync(INetworkMessage networkMessage, CancellationToken token = default);

        /// <summary>
        /// Получить сообщение
        /// Поддерживает ожидание
        /// </summary>
        /// <returns></returns>
        Task<INetworkObject> ReceiveAsync(CancellationToken token = default);

        /// <summary>
        /// Прекратить коммуницирование
        /// </summary>
        void Stop();
    }
}