using NetworkMessage.CommandsResaults;

namespace NetworkMessage.Commands
{
    public interface INetworkCommand : INetworkObject
    {
        Task<INetworkCommandResult> Do(CancellationToken token = default, params object[] objects);
    }
}