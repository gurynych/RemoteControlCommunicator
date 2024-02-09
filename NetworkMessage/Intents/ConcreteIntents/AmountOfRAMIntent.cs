using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
	public class AmountOfRAMIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public AmountOfRAMIntent()
        {
            IntentType = nameof(AmountOfRAMIntent);
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateAmountOfRAMCommand();
        }
    }
}
