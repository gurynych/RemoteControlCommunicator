namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class BatteryChargeResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public byte ButteryChargePercent { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private BatteryChargeResult()
        {
        }

        public BatteryChargeResult(byte butteryChargePercent)
        {
            if (butteryChargePercent == default) throw new ArgumentNullException(nameof(butteryChargePercent));
            ButteryChargePercent = butteryChargePercent;
        }

        public BatteryChargeResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
