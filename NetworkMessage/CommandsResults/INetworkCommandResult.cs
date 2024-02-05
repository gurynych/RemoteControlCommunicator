namespace NetworkMessage.CommandsResults
{
    public interface INetworkCommandResult : INetworkObject
    {
        [Newtonsoft.Json.JsonProperty]
        public string ErrorMessage { get; }

        [Newtonsoft.Json.JsonProperty]
        public Exception Exception { get; }
    }
}
