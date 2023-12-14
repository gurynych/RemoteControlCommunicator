using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    internal class Intent : BaseIntent
    {
        [Newtonsoft.Json.JsonProperty]
        public override string IntentType { get; protected set; }

        [Newtonsoft.Json.JsonConstructor]
        private Intent()
        {            
        }

        public Intent(string intentType)
        {
            IntentType = intentType;
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return default;
        }
    }
}
