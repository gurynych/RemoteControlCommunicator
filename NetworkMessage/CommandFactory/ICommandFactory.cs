using NetworkMessage.Commands;

namespace NetworkMessage.CommandFactory
{
    /// <summary>
    /// Интерфейс абстрактной фабрики, определяющий методы создания команд
    /// </summary>
    public interface ICommandFactory
    {
        public INetworkCommand CreateAmountOfOccupiedRAMCommand();
        public INetworkCommand CreateAmountOfRAMCommand();
        public INetworkCommand CreateBatteryChargePersentageCommand();
        public INetworkCommand CreateDirectoryInfoCommand(string path);
        public INetworkCommand CreateDownloadDirectoryCommand(string path);
        public INetworkCommand CreateDownloadFileCommand(string path);
        public INetworkCommand CreateDrivesInfoCommand();
        public INetworkCommand CreateFileInfoCommand(string path);
        public INetworkCommand CreateGuidCommand();
        public INetworkCommand CreateMacAddressCommand();
        public INetworkCommand CreateNestedDirectoriesInfoCommand(string path);
        public INetworkCommand CreateNestedFilesInfoCommand(string path);
        public INetworkCommand CreatePercentageOfCPUUsageCommand();
        public INetworkCommand CreateScreenshotCommand();
        public INetworkCommand CreateRunningProgramsCommand();
        public INetworkCommand CreateStartProgramCommand(string path);
    }
}
