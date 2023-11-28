using Microsoft.Win32;
using NetworkMessage.CommandsResults;

namespace NetworkMessage.Commands
{
    public class DeviceGuidCommand : NetworkCommandBase
    {
        public override async Task<NetworkCommandResultBase> DoAsync(CancellationToken token = default, params object[] objects)
        {
            string location = @"SOFTWARE\Microsoft\Cryptography";
            string name = "MachineGuid";
            
            using RegistryKey localMachineX64View =
                RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
            using RegistryKey rk = localMachineX64View.OpenSubKey(location) ?? 
                throw new KeyNotFoundException($"Key Not Found: {location}");

            object machineGuid = rk.GetValue(name) ?? 
                throw new IndexOutOfRangeException($"Index Not Found: {name}");

            DeviceGuidResult hwidResult = new DeviceGuidResult(machineGuid.ToString());
            return await Task.FromResult(hwidResult);

            //string hwid = libc.hwid.HwId.Generate();
            //return Task.FromResult(new HwidResult(hwid));
        }
    }
}
