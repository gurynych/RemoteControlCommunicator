using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Intents
{
    public class DownloadFileIntent : BaseIntent
    {
        public string Path { get; }

        public override string IntentType { get; protected set; }

        public DownloadFileIntent(string path)
        {
            Path = path;
            IntentType = nameof(DownloadFileIntent);
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateDownloadFileCommand(Path);
        }
    }
}
