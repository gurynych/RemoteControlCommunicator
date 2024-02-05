using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Intents.ConcreteIntents
{
    internal class DirectoryInfoIntent : BaseIntent
    {
        public string Path { get; }

        public override string IntentType { get; protected set; }

        public DirectoryInfoIntent(string path)
        {
            Path = path;
            IntentType = nameof(DirectoryInfoIntent);
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateDirectoryInfoCommand(Path);
        }
    }
}
