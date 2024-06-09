using NetworkMessage.CommandsResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Commands
{
    public interface INetworkCommand
    {
        Task<BaseNetworkCommandResult> ExecuteAsync(CancellationToken token = default, params object[] objects);
    }
}
