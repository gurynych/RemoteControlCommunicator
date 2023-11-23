using Microsoft.VisualBasic.Devices;
using NetworkMessage.CommandsResaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Commands
{
    public class AmountOfRAMCommand : NetworkCommandBase
    {
        public override Task<INetworkCommandResult> Do(CancellationToken token = default, params object[] objects)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                ComputerInfo computerInfo = new ComputerInfo();
                float totalMemoryAmount =(float) Math.Round((computerInfo.TotalPhysicalMemory / 1024.0 / 1024.0 / 1024.0), 1);
                INetworkCommandResult totalMemory = new AmountOfRAMResult(totalMemoryAmount);
                return Task.FromResult(totalMemory);
            }

            return default;
        }
    }
}
