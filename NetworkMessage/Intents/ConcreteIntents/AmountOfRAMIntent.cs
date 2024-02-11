using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
	public class AmountOfRAMIntent : BaseIntent
    { 
        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateAmountOfRAMCommand();
        }
    }
}
