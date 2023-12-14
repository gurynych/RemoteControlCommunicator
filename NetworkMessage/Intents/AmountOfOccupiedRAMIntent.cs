using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public class AmountOfOccupiedRAMIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public AmountOfOccupiedRAMIntent()
        {
            IntentType = nameof(AmountOfOccupiedRAMIntent);
        }        

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateAmountOfOccupiedRAMCommand();
        }
    }
}
