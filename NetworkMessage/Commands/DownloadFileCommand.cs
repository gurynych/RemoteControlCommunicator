using NetworkMessage.CommandsResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Commands
{
    public class DownloadFileCommand : NetworkCommandBase
    {
        public string Path { get; private set; }

        public DownloadFileCommand(string path) {
            Path = path;
        }

        public override async Task<NetworkCommandResultBase> DoAsync(CancellationToken token = default, params object[] objects)
        {
            if(File.Exists(Path))
            {
                byte[] file = await File.ReadAllBytesAsync(Path);
                NetworkCommandResultBase loadedFileResult = new DownloadedFileResult(file);
                return loadedFileResult;
            }
            return default;
        }
    }
}
