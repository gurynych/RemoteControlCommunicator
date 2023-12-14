using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public class NestedDirectoriesInfoIntent : BaseIntent
    {
        public string Path { get; }

        public override string IntentType { get; protected set; }

        public NestedDirectoriesInfoIntent(string path)
        {
            Path = path;
            IntentType = nameof(NestedDirectoriesInfoIntent);
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateNestedDirectoriesInfoCommand(Path);
        }
    }
}
