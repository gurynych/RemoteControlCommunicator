namespace NetworkMessage.CommandsResults
{
    public class AmountOfRAMResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public float AmountOfRAM { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private AmountOfRAMResult()
        {            
        }
        
        public AmountOfRAMResult(float amountOfRAM)
        {
            if (amountOfRAM == default) throw new ArgumentNullException(nameof(amountOfRAM));
            AmountOfRAM = amountOfRAM;
        }

        public AmountOfRAMResult(string errorMessage, Exception exception = null)
            : base(errorMessage, exception)
        {
        }
    }
}
