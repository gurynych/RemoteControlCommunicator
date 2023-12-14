namespace NetworkMessage.CommandsResults
{
    public class FileInfoResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public Models.MyFileInfo FileInfo { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private FileInfoResult()
        {
        }

        public FileInfoResult(Models.MyFileInfo fileInfo)
        {
            FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
        }

        public FileInfoResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
