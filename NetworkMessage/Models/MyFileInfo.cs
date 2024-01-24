namespace NetworkMessage.Models
{
    public class MyFileInfo : IInfoOfExplorerObject
    {
        [Newtonsoft.Json.JsonProperty]
        public string Name { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public DateTime CreationDate { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public DateTime ChangingDate { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public long Size { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public string FullName { get; protected set; }

        [Newtonsoft.Json.JsonConstructor]
        private MyFileInfo()
        {
        }

        public MyFileInfo(string name, DateTime creationDate, DateTime changingDate, long fileLength, string fullName)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentNullException(nameof(fullName));
            if (fileLength <= 0) throw new ArgumentOutOfRangeException(nameof(fileLength));
            Name = name;
            CreationDate = creationDate;
            ChangingDate = changingDate;
            Size = fileLength;
            FullName = fullName;
        }
    }
}
