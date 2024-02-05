namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class MacAddressResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public string MacAddress { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private MacAddressResult()
        {
        }

        public MacAddressResult(string macAddress)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(macAddress, nameof(macAddress));
            MacAddress = macAddress;
        }

        public MacAddressResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
