using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public class NestedFilesInfoIntent : BaseIntent
    {
        public string Path { get; }

        public override string IntentType { get; protected set; }     

        public NestedFilesInfoIntent(string path)
        {
            Path = path;
            IntentType = nameof(NestedFilesInfoIntent);
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateNestedFilesInfoCommand(Path);
        }
    }
}
