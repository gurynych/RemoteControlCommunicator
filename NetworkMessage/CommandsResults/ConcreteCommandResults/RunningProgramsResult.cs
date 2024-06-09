using NetworkMessage.DTO;
using Newtonsoft.Json;

namespace NetworkMessage.CommandsResults.ConcreteCommandResults;

public class RunningProgramsResult : BaseNetworkCommandResult
{
    [JsonProperty]
    public IEnumerable<ProgramInfoDTO> RunningPrograms { get; private set; }

    [JsonConstructor]
    private RunningProgramsResult()
    {
    }

    public RunningProgramsResult(IEnumerable<ProgramInfoDTO> runningPrograms)
    {
        this.RunningPrograms = runningPrograms ?? throw new ArgumentNullException(nameof (runningPrograms));
    }
}