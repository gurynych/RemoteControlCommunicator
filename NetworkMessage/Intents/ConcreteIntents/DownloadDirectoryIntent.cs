using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class DownloadDirectoryIntent : BaseIntent
    {
        public string Path { get; }

        public override string IntentType { get; protected set; }

        public DownloadDirectoryIntent(string path)
        {
            Path = path;
            IntentType = nameof(DownloadDirectoryIntent);
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateDownloadDirectoryCommand(Path);
        }
    }
}
