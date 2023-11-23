using NetworkMessage.CommandsResaults;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Commands
{
    public class ButteryChargePersentageCommand : NetworkCommandBase
    {
        public override Task<INetworkCommandResult> Do(CancellationToken token = default, params object[] objects)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                PowerStatus powerStatus =SystemInformation.PowerStatus;
                INetworkCommandResult butteryChargePercent;

                if (powerStatus.BatteryChargeStatus is BatteryChargeStatus.NoSystemBattery)
                {
                    butteryChargePercent = new ButteryChargeResult((byte)100);
                    return Task.FromResult(butteryChargePercent);
                }

                butteryChargePercent = new ButteryChargeResult((byte)(powerStatus.BatteryLifePercent * 100));
                return Task.FromResult(butteryChargePercent);
            }

            return default;
        }
    }
}
