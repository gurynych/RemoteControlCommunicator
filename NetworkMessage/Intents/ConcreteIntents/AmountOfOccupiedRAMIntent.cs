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

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateAmountOfOccupiedRAMCommand();
        }
    }
}
