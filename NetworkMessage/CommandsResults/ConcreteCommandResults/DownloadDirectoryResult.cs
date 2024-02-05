using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.CommandsResults.ConcreteCommandResults
{
    public class DownloadDirectoryResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public string Path { get; }

        [Newtonsoft.Json.JsonConstructor]
        private DownloadDirectoryResult()
        {
        }

        public DownloadDirectoryResult(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));
            Path = path;
        }

        public DownloadDirectoryResult(string errorMessage, Exception exception = null)
        : base(errorMessage, exception)
        {
        }

        public override Stream ToStream()
        {
            FileStream fs = File.Create(System.IO.Path.GetTempFileName());
            ZipFile.CreateFromDirectory(Path, fs, CompressionLevel.Fastest, true);
            return fs;
        }
    }
}
