using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public interface IIntent : INetworkObject
    {
        string IntentType { get; }

        /// <summary>
        /// Создание команды, используя абстрактную фабрику
        /// </summary>
        /// <param name="commandFactory"></param>
        /// <returns></returns>
        INetworkCommand CreateCommand(ICommandFactory commandFactory);
    }
}
