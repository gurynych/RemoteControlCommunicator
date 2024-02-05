using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class FileInfoIntent : BaseIntent
    {
        public string Path { get; }

        public override string IntentType { get; protected set; }

        public FileInfoIntent(string path)
        {
            Path = path;
            IntentType = nameof(FileInfoIntent);
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateFileInfoCommand(Path);
        }
    }
}
