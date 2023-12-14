using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
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

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateFileInfoCommand(Path);
        }
    }
}
