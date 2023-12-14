using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Intents
{
    public class ButteryChargePersentageIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public ButteryChargePersentageIntent()
        {
            IntentType = nameof(ButteryChargePersentageIntent);
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            throw new NotImplementedException();
        }
    }
}
