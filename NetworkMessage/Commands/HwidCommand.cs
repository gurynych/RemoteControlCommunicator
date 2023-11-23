using NetworkMessage.CommandsResaults;

namespace NetworkMessage.Commands
{
    public class HwidCommand : NetworkCommandBase
    {
        public override async Task<INetworkCommandResult> Do(CancellationToken token = default, params object[] objects)
        {
            string hwid = libc.hwid.HwId.Generate();
            return await Task.FromResult(new HwidResult(hwid));
        }
    }
}
