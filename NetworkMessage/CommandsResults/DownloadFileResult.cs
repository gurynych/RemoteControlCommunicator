using Newtonsoft.Json.Bson;
using System.IO;

namespace NetworkMessage.CommandsResults
{
    public class DownloadFileResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonIgnore]
        private readonly string path;

        /*public DownloadFileResult(byte[] file)
        {
            if (file == null || file.Length == 0) throw new ArgumentNullException(nameof(file));
            File = file;
        }*/

        public DownloadFileResult(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentNullException(nameof(path));
            this.path = path;
        }

        public DownloadFileResult(string errorMessage, Exception exception = null)
        : base(errorMessage, exception)
        { 
        }

        public override Stream ToStream()
        {
            return File.OpenRead(path);
        }
    }
}
