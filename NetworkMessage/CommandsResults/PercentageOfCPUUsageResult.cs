namespace NetworkMessage.CommandsResults
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
            if (persentageOfCPUUsage < 0) throw new ArgumentOutOfRangeException(nameof(persentageOfCPUUsage));
            PercentageOfCPUUsage = persentageOfCPUUsage;
        }

        public PercentageOfCPUUsageResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }        
    }
}
