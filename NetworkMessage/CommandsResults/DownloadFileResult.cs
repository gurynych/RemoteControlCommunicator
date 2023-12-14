using System.IO;

namespace NetworkMessage.CommandsResults
{
    public class DownloadFileResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public byte[] File { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private DownloadFileResult()
        {            
        }

        public DownloadFileResult(byte[] file)
        {
            if (file == null || file.Length == 0) throw new ArgumentNullException(nameof(file));
            File = file;
        }
        public DownloadFileResult(string errorMessage, Exception exception = null)
        : base(errorMessage, exception)
        { 
        }
    }
}
