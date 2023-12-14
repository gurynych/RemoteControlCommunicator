namespace NetworkMessage.CommandsResults
{
    public class DirectoryInfoResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public Models.MyDirectoryInfo DirectoryInfo { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private DirectoryInfoResult()
        {            
        }

        public DirectoryInfoResult(Models.MyDirectoryInfo directoryInfo)
        {
            DirectoryInfo = directoryInfo ?? throw new ArgumentNullException(nameof(directoryInfo));
        }

        public DirectoryInfoResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
