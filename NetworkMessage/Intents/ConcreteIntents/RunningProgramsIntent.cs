using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents;

public class RunningProgramsIntent : BaseIntent
{
    public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
    {
        return commandFactory.CreateRunningProgramsCommand();
    }
}