using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class NestedDirectoriesInfoIntent : BaseIntent
    {
        public string Path { get; }        

        public NestedDirectoriesInfoIntent(string path) : base()
        {
            Path = path;            
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateNestedDirectoriesInfoCommand(Path);
        }
    }
}
