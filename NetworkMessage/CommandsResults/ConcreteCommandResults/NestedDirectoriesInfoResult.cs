using NetworkMessage.DTO;

namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class NestedDirectoriesInfoResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public IEnumerable<FileInfoDTO> NestedDirectoriesInfo { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private NestedDirectoriesInfoResult()
        {
        }

        public NestedDirectoriesInfoResult(IEnumerable<FileInfoDTO> nestedDirectoriesInfo)
        {
            NestedDirectoriesInfo = nestedDirectoriesInfo ?? throw new ArgumentNullException(nameof(nestedDirectoriesInfo));
        }

        public NestedDirectoriesInfoResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
