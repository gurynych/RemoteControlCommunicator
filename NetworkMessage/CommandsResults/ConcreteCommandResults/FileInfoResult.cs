namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class FileInfoResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public DTO.FileInfoDTO FileInfo { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private FileInfoResult()
        {
        }

        public FileInfoResult(DTO.FileInfoDTO fileInfo)
        {
            FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
        }

        public FileInfoResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
