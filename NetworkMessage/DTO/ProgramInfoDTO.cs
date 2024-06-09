using Newtonsoft.Json;

namespace NetworkMessage.DTO;

public class ProgramInfoDTO
{
    [JsonProperty]
    public string ProgramName { get; private set; }

    [JsonProperty]
    public long MemoryByte { get; private set; }

    [JsonProperty]
    public string ProgramPath { get; private set; }

    [JsonConstructor]
    private ProgramInfoDTO()
    {
    }

    public ProgramInfoDTO(string programName, long memoryByte, string programPath)
    {
        this.ProgramName = programName;
        this.MemoryByte = memoryByte;
        this.ProgramPath = programPath;
    }
}