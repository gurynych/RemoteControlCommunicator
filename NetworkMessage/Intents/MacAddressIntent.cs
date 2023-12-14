using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents
{
    public class MacAddressIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public MacAddressIntent()
        {
            IntentType = nameof(MacAddressIntent);
        }

        public override BaseNetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateMacAddressCommand();
        }
    }
}
