namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class AmountOfOccupiedRAMResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public float AmountOfOccupiedRAM { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private AmountOfOccupiedRAMResult()
        {
        }

        public AmountOfOccupiedRAMResult(float amountOfOccupiedRAM)
        {
            AmountOfOccupiedRAM = amountOfOccupiedRAM;
        }

        public AmountOfOccupiedRAMResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
