namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class NestedFilesInfoResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public IEnumerable<Models.MyFileInfo> NestedFilesInfo { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private NestedFilesInfoResult()
        {
        }

        public NestedFilesInfoResult(IEnumerable<Models.MyFileInfo> nestedFilesInfo)
        {
            NestedFilesInfo = nestedFilesInfo ?? throw new ArgumentNullException(nameof(nestedFilesInfo));
        }

        public NestedFilesInfoResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
