using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class DownloadFileIntent : BaseIntent
    {
        public string Path { get; }        

        public DownloadFileIntent(string path) : base()
        {
            Path = path;           
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateDownloadFileCommand(Path);
        }
    }
}
