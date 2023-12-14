using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.CommandsResults
{
    internal class DownloadDirectoryResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public byte[] File { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private DownloadDirectoryResult()
        {
        }

        public DownloadDirectoryResult(byte[] file)
        {
            if (file == null || file.Length == 0) throw new ArgumentNullException(nameof(file));
            File = file;
        }
        public DownloadDirectoryResult(string errorMessage, Exception exception = null)
        : base(errorMessage, exception)
        {
        }
    }
}
