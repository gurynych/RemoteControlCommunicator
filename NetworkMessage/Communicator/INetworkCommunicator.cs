using NetworkMessage.CommandsResults;
using NetworkMessage.Intents;

namespace NetworkMessage.Communicator
{
    public interface INetworkCommunicator : IDisposable
    {
        bool IsConnected { get; }

        /// <summary>
        /// Пытается подключиться к указанному серверу и порту. Поддерживает ожидание.
        /// </summary>
        /// <param name="serverIP">IP-адрес сервера для подключения</param>
        /// <param name="serverPort">Порт сервера, к которому будет проихсодить подключение</param>        
        /// <returns>В случае успешного подключения возвращает true, в противном случае - false</returns>
        Task<bool> ConnectAsync(string serverIP, int serverPort, CancellationToken token = default);

        /// <summary>
        /// Пытается произвести переподлючение к серверу, в случае успеха происходит handshake. Поддерживает ожидание
        /// </summary>
        /// <param name="serverIP">IP-адрес сервера для подключения</param>
        /// <param name="serverPort">Порт сервера, к которому будет проихсодить подключение</param>        
        /// <param name="progress">Интерфейс для информирования состояния текущей операции</param>        
        /// <returns>В случае успешного обмена данными возвращает true, в противном случае - false</returns>
        Task<bool> ReconnectWithHandshakeAsync(string serverIP, int serverPort, IProgress<long> progress = null, CancellationToken token = default);

        /// <summary>
        /// Отправить данные по сети. Поддерживает ожидание
        /// </summary>
        /// <param name="netObject">Представляет собой данные для отправки</param>
        /// <param name="progress">Вызывается метод Report для уведомлениче об изменении общих отправленных байт</param>        
        Task SendObjectAsync(INetworkObject netObject, IProgress<long> progress = null, CancellationToken token = default);

        /// <summary>
        /// Получение сырых данных в сети из определенного формата сообщения. Поддерживает ожидание
        /// </summary>
        /// <param name="progress">Вызывается метод Report для уведомлениче об изменении количества полученных байт</param>        
        /// <returns>Полученные данные в виде массива байт</returns>        

        public Task ReceiveStreamAsync(Stream streamToWrite, IProgress<long> progress = null, CancellationToken token = default);

        /// <summary>
        /// Получить намерение. Поддерживает ожидание
        /// </summary>
        /// <param name="progress">Вызывается метод Report для уведомлениче об изменении общих полученных байт</param>        
        /// <returns>В случае успещного парса - определенное намерение, приведенное к базовому классу, в противном случае null</returns>
        Task<BaseIntent> ReceiveAsync(IProgress<long> progress = null, CancellationToken token = default);

        /// <summary>
        /// Полулучить конкретный объект из сети. Поддерживает ожидание
        /// </summary>       
        /// <param name="progress">Вызывается метод Report для уведомлениче об изменении количества полученных байт</param>
        /// <returns></returns>
        Task<TNetworkObject> ReceiveAsync<TNetworkObject>(IProgress<long> progress = null, CancellationToken token = default)
            where TNetworkObject : INetworkObject;

        /// <summary>
        /// Обмен необходимыми данными между сервером и клиентом. Поддерживает ожидание
        /// </summary>        
        /// <returns>В случае успешного обмена данными возвращает true, в противном случае - false</returns>
        Task<bool> HandshakeAsync(IProgress<long> progress = null, CancellationToken token = default);
    }
}