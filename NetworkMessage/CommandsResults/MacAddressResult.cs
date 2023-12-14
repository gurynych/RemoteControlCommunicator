namespace NetworkMessage.CommandsResults
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
            if (string.IsNullOrEmpty(macAddress)) throw new ArgumentNullException(nameof(macAddress));
            MacAddress = macAddress;
        }

        public MacAddressResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
