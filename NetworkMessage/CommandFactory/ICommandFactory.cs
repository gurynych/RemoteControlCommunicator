using NetworkMessage.Commands;

namespace NetworkMessage.CommandFactory
{
    public interface ICommandFactory
    {
        BaseNetworkCommand CreateGuidCommand(); //i+ r+
        BaseNetworkCommand CreateFileInfoCommand(string path); //i+ r+
        BaseNetworkCommand CreateDownloadFileCommand(string path); //i+ r+
        BaseNetworkCommand CreateNestedFilesInfoCommand(string path); //i+ r+
        BaseNetworkCommand CreateDirectoryInfoCommand(string path); //i+ r+
        BaseNetworkCommand CreateDownloadDirectoryCommand(string path); //i+ r+
        BaseNetworkCommand CreateNestedDirectoriesInfoCommand(string path); //i+ r+
        BaseNetworkCommand CreatePercentageOfCPUUsageCommand(); //i+ r+
        BaseNetworkCommand CreateAmountOfOccupiedRAMCommand(); //i+ r+
        BaseNetworkCommand CreateAmountOfRAMCommand(); //i+ r+
        BaseNetworkCommand CreateBatteryChargePersentageCommand(); //i+ r+
        BaseNetworkCommand CreateMacAddressCommand(); //i+ r+
        BaseNetworkCommand CreateScreenshotCommand(); //i+ r+
    }
}
