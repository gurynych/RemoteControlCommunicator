using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class PercentageOfCPUUsageIntent : BaseIntent
    { 
        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreatePercentageOfCPUUsageCommand();
        }
    }
}
