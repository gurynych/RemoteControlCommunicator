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
        
        public DirectoryInfoIntent(string path) : base()
        {
            Path = path;
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateDirectoryInfoCommand(Path);
        }
    }
}
