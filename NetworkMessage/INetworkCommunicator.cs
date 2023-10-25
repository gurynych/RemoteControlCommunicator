using NetworkMessage.CommandsResaults;

namespace NetworkMessage
{
    public interface INetworkCommunicator
    {
        public void Send(INetworkObject networkObject);

        public void SendPublicKey(PublicKeyResult publicKeyResult);

        public Task SendPublicKeyAsync(PublicKeyResult publicKeyResult, CancellationToken token = default);

        public INetworkObject Receive();

        public PublicKeyResult ReceivePublicKey();

        public Task<PublicKeyResult> ReceivePublicKeyAsync(CancellationToken token = default);



        Task SendAsync(INetworkObject networkObject, CancellationToken token = default);

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