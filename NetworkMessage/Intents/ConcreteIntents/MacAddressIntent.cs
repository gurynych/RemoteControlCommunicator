using NetworkMessage.CommandFactory;
using NetworkMessage.Commands;

namespace NetworkMessage.Intents.ConcreteIntents
{
    public class MacAddressIntent : BaseIntent
    {
        public override string IntentType { get; protected set; }

        public MacAddressIntent()
        {
            IntentType = nameof(MacAddressIntent);
        }

        public override INetworkCommand CreateCommand(ICommandFactory commandFactory)
        {
            return commandFactory.CreateMacAddressCommand();
        }
    }
}
