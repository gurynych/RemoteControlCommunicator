using NetworkMessage.Commands;

namespace NetworkMessage.CommandFactory
{
    public interface ICommandFactory
    {
        INetworkCommand CreateGuidCommand();
        INetworkCommand CreateFileInfoCommand(string path);
        INetworkCommand CreateDownloadFileCommand(string path);
        INetworkCommand CreateNestedFilesInfoCommand(string path);
        INetworkCommand CreateDirectoryInfoCommand(string path);
        INetworkCommand CreateDownloadDirectoryCommand(string path);
        INetworkCommand CreateNestedDirectoriesInfoCommand(string path);
        INetworkCommand CreatePercentageOfCPUUsageCommand();
        INetworkCommand CreateAmountOfOccupiedRAMCommand();
        INetworkCommand CreateAmountOfRAMCommand();
        INetworkCommand CreateBatteryChargePersentageCommand();
        INetworkCommand CreateMacAddressCommand();
        INetworkCommand CreateScreenshotCommand();
    }
}
