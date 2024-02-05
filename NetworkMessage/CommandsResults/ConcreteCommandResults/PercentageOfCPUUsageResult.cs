namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class PercentageOfCPUUsageResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public byte PercentageOfCPUUsage { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private PercentageOfCPUUsageResult()
        {
        }

        public PercentageOfCPUUsageResult(byte persentageOfCPUUsage)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(PercentageOfCPUUsage, nameof(PercentageOfCPUUsage));
            PercentageOfCPUUsage = persentageOfCPUUsage;
        }

        public PercentageOfCPUUsageResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
