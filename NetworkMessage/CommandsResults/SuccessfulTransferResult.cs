using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.CommandsResults
{
    public class SuccessfulTransferResult : BaseNetworkCommandResult
    {
        [Newtonsoft.Json.JsonProperty]
        public bool IsSuccessful { get; private set; }

        [Newtonsoft.Json.JsonConstructor]
        private SuccessfulTransferResult()
        {            
        }

        public SuccessfulTransferResult(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }
    }
}
