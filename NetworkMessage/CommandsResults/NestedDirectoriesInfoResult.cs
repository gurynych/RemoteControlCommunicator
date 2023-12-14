namespace NetworkMessage.CommandsResults
{
    public class NestedDirectoriesInfoResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public IEnumerable<Models.MyDirectoryInfo> NestedDirectoriesInfo { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private NestedDirectoriesInfoResult()
        {
        }

        public NestedDirectoriesInfoResult(IEnumerable<Models.MyDirectoryInfo> nestedDirectoriesInfo)
        {            
            NestedDirectoriesInfo = nestedDirectoriesInfo ?? throw new ArgumentNullException(nameof(nestedDirectoriesInfo));
        }

        public NestedDirectoriesInfoResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
