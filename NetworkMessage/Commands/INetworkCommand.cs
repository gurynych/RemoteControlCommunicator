using NetworkMessage.CommandsResaults;

namespace NetworkMessage.Commands
{
    public interface INetworkCommand : INetworkObject
    {
        Task<INetworkCommandResult> Do(params object[] objects);
    }
}