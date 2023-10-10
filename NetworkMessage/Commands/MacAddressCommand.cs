using NetworkMessage.CommandsResaults;
using System.Net.NetworkInformation;

namespace NetworkMessage.Commands
{
    public class MacAddressCommand : NetworkCommandBase
    {
        public override async Task<INetworkCommandResult> Do(params object[] objects)
        {
            string macAddress = NetworkInterface.GetAllNetworkInterfaces()
                .Where(x => x.OperationalStatus == OperationalStatus.Up)
                .OrderByDescending(x => x.GetIPStatistics().BytesSent + x.GetIPStatistics().BytesReceived)
                .FirstOrDefault()?.GetPhysicalAddress().ToString();

            return await Task.FromResult(new MacAddressResult(macAddress));
        }
    }
}
