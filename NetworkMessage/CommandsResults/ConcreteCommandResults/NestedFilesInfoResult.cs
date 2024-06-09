namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class NestedFilesInfoResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public IEnumerable<DTO.FileInfoDTO> NestedFilesInfo { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private NestedFilesInfoResult()
        {
        }

        public NestedFilesInfoResult(IEnumerable<DTO.FileInfoDTO> nestedFilesInfo)
        {
            NestedFilesInfo = nestedFilesInfo ?? throw new ArgumentNullException(nameof(nestedFilesInfo));
        }

        public NestedFilesInfoResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
