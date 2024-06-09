using Newtonsoft.Json;

namespace NetworkMessage.DTO
{
    public class FileInfoDTO
    {
        [JsonProperty] public string Name { get; private set; }

        [JsonProperty] public DateTime? CreationDate { get; private set; }

        [JsonProperty] public DateTime? ChangingDate { get; private set; }

        [JsonProperty] public string FullName { get; private set; }

        [JsonProperty] public long? FileLength { get; private set; }

        [JsonProperty] public string FileType { get; set; }

        [JsonConstructor]
        private FileInfoDTO()
        {
        }

        public FileInfoDTO(string name, DateTime? creationDate, DateTime? changingDate, long? fileLength, string fullName, FileType fileType)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
            ArgumentException.ThrowIfNullOrWhiteSpace(fullName, nameof(fullName));
            if (fileLength.HasValue)
                ArgumentOutOfRangeException.ThrowIfNegative(fileLength.Value, nameof(fileLength));
            Name = name;
            CreationDate = creationDate;
            ChangingDate = changingDate;
            FileLength = fileLength;
            FullName = fullName;
            FileType = Enum.GetName(fileType);
        }
    }

    public enum FileType
    {
        File,
        Directory,
        Drive
    }
}