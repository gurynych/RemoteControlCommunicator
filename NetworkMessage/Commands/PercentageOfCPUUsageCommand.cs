using NetworkMessage.CommandsResaults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Commands
{
    public class PercentageOfCPUUsageCommand : NetworkCommandBase
    {
        public override Task<INetworkCommandResult> Do(CancellationToken token = default, params object[] objects)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                float sum = 0;
                for (int i = 0; i < 100; i++)
                {
                    float cpuUsage = cpuCounter.NextValue();
                    sum += cpuUsage;
                    Thread.Sleep(10);
                }
                INetworkCommandResult percentageOfCPUUsageResult = new PercentageOfCPUUsageResult((byte)(sum / 100));
                return Task.FromResult(percentageOfCPUUsageResult);
            }
            return default;
        }
    }
}
