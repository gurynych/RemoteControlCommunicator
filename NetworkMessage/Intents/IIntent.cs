using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public interface IIntent : INetworkObject
    {
        string IntentType { get; }

        INetworkCommand CreateCommand(ICommandFactory commandFactory);
    }
}
