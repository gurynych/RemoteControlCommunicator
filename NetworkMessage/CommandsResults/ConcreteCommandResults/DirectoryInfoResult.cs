using NetworkMessage.DTO;

namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class DirectoryInfoResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public FileInfoDTO DirectoryInfo { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private DirectoryInfoResult()
        {
        }

        public DirectoryInfoResult(FileInfoDTO directoryInfo)
        {
            DirectoryInfo = directoryInfo ?? throw new ArgumentNullException(nameof(directoryInfo));
        }

        public DirectoryInfoResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
