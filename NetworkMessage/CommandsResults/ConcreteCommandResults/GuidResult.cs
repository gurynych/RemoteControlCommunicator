using System.Text;

namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class GuidResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public string Guid { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private GuidResult()
        {
        }

        public GuidResult(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));
            Guid = guid;
        }

        public GuidResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
