using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Intents
{
    public class AmountOfRAMIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public AmountOfRAMIntent()
        {
            IntentType = nameof(AmountOfRAMIntent);
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateAmountOfRAMCommand();
        }
    }
}
