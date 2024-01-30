using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkMessage.Intents
{
    public class BatteryChargePersentageIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public BatteryChargePersentageIntent()
        {
            IntentType = nameof(BatteryChargePersentageIntent);
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateBatteryChargePersentageCommand();
        }
    }
}
