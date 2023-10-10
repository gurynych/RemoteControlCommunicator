namespace NetworkMessage
{
    public interface INetworkCommunicator
    {
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