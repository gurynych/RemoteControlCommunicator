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
        public string FullName { get; protected set; }

        [Newtonsoft.Json.JsonProperty]
        public long FileLength { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private MyFileInfo()
        {
        }

        public MyFileInfo(string name, DateTime creationDate, DateTime changingDate, long fileLength, string fullName)
        {
			ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
			ArgumentException.ThrowIfNullOrWhiteSpace(fullName, nameof(fullName));
			ArgumentOutOfRangeException.ThrowIfNegative(fileLength);
			Name = name;
            CreationDate = creationDate;
            ChangingDate = changingDate;
            FileLength = fileLength;
            FullName = fullName;
        }
    }
}
