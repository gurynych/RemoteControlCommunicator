using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class FileInfoIntent : BaseIntent
    {
        public string Path { get; }        

        public FileInfoIntent(string path) : base()
        {
            Path = path;            
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateFileInfoCommand(Path);
        }
    }
}
