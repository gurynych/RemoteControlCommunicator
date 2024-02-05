using System.Text;

namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class DeviceGuidResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public string Guid { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private DeviceGuidResult()
        {
        }

        public DeviceGuidResult(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentNullException(nameof(guid));
            Guid = guid;
        }

        public DeviceGuidResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
