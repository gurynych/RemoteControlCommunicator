namespace NetworkMessage.Models
{
    public class MyDirectoryInfo : IInfoOfExplorerObject
    {
        [Newtonsoft.Json.JsonProperty]
        public string Name { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public DateTime CreationDate { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public DateTime ChangingDate { get; private set; }

        [Newtonsoft.Json.JsonProperty]
        public string FullName { get; protected set; }

        [Newtonsoft.Json.JsonConstructor]
        public MyDirectoryInfo()
        {            
        }

        public MyDirectoryInfo(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        public MyDirectoryInfo(string name, DateTime creationDate, DateTime changingDate, string fullName)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(fullName)) throw new ArgumentNullException(nameof(fullName));
            Name = name;
            CreationDate = creationDate;
            ChangingDate = changingDate;
            FullName = fullName;
        }
    }
}
