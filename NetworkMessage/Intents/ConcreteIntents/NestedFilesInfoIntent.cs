using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class NestedFilesInfoIntent : BaseIntent
    {
        public string Path { get; }        

        public NestedFilesInfoIntent(string path) : base()
        {
            Path = path;
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateNestedFilesInfoCommand(Path);
        }
    }
}
